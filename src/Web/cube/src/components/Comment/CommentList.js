const signalR = require("@microsoft/signalr");

const CommentThumbupType = 0;
const CommentMessageType = 1;

const AddOperation = 'add';
const UpdateOperation = 'update';
const DeleteOperation = 'delete';

export default {
  props: ['boardItem','boardItemType','listOfItems'],
  data() {
    return {
      loading: false,
      userName: null,
      commentItemTextChanged: false,
      boardId: null,
      connection: "",
    };
  },
  created(){
    this.boardId = this.$route.params.boardId;
    this.userName = localStorage.getItem('LOGINUSER').toUpperCase();

    this.connection = new signalR.HubConnectionBuilder()
        .withUrl("http://techgroupdockerdc:5050/BoardHub", {})
        .configureLogging(signalR.LogLevel.Error)
        .build();

    this.connection.start();
  },
  destroyed() {
    if (this.connection != null) {
      console.log("Hub for CommentList with " + this.connection.connectionId + "is stopped");
      this.connection.stop();
    }
  },
  methods:{
    getUserAvatar(userName) {
      let userAvatar = ""
      try {
        userAvatar = require('../../assets/Person/' + userName.toLowerCase() + '.jpg')
        return userAvatar
      } catch (e) {
        userAvatar = require('../../assets/Person/cube.jpg')
        return userAvatar
      }
    },
    canEditComment(comment) {
      if (comment.CreatedUser == this.userName) {
        return true;
      }

      return false;
    },
    canDeleteComment(message) {

      if (message.CreatedUser == this.userName) {
        return true;
       }

      return false;
    },
    commentItemChanged() {
      this.commentItemTextChanged = true;
    },

    updateCommentItem(comment) {
      if (!this.commentItemTextChanged) {
        return;
      }

      this.axios(
        {
          method: 'put',
          url: '/Comment',
          data: {
            id: comment.Id,
            detail: comment.Detail,
            type: comment.Type,
            createduser: comment.CreatedUser,
            boarditemid: comment.BoardItemId
          }
        }).then(() => {
          this.commentItemTextChanged = false;
        }).then(() => {
          this.renderFunc(comment.Detail + ' is updated successfully.');
        }).then(() => {

          var context = {
            Operation: UpdateOperation,
            BoardId: this.boardId,
            Type: this.boardItemType,
            Comment: comment
          };
          this.sendCommentMsg(context);
        });
    },

    addCommentItem(boardItem) {
      this.addComment(this.boardItemType, boardItem, CommentMessageType);
    },
    addComment(type, boardItem, thumpType) {

      if (!boardItem.Comment.Detail || !boardItem.Comment.Detail.trim()) {
        return;
      }

      if (this.loading) {
        return;
      }

      this.loading = true;

      this.axios({
        method: 'post',
        url: '/Comment',
        data: {
          BoardItemId: boardItem.Id,
          Type: thumpType,
          Detail: boardItem.Comment.Detail,
          CreatedUser: this.userName
        }
      }).then(res => {
        var comment = {
          Id: res.data,
          CreatedUser: this.userName,
          Type: thumpType,
          Detail: boardItem.Comment.Detail,
          DateCreated: new Date((new Date()).getTime() - 8*60*60*1000),
          BoardItemId: boardItem.Id
        };
        var context = {
          Operation: AddOperation,
          BoardId: this.boardId,
          Type: type,
          Comment: comment
        };
        this.sendCommentMsg(context);

        boardItem.Comment.Detail = "";
        this.loading = false;
      }).catch(error => {
        this.$Message.error('Failed to add comment item. Error:' + error);
        this.loading = false;
      });
    },
    sendCommentMsg(context) {
      this.connection.invoke("SendCommentMessage", context);
    },
    thumbsUpUserNames(thumbsUp) {
      var names = '';

      if (thumbsUp.length > 0) {
        for (var i = 0; i < thumbsUp.length; i++) {
          names = names + thumbsUp[i].CreatedUser + '\n';
        }
      }
      else {
        names = 'Like';
      }

      return names;
    },

    thumbsUpCount(thumbsUp) {

      var len = thumbsUp.length;
      if (len > 0) {
        return len;
      }

      return '';
    },

    thumbsUpClass(thumbsUp) {
      var style = 'fa fa-2x';
      var index = thumbsUp.find(item => item.CreatedUser == this.userName);
      if (index != null) {
        style += ' fa-thumbs-up';
      }
      else {
        style += ' fa fa-thumbs-o-up';
      }

      if (this.thumbsUpCount(thumbsUp) > 0) {
        style += ' commentActionHighlightStyle';
      }
      else {
        style += ' commentActionStyle';
      }
      return style;
    },

    commentContentClass(message) {
      var style = '';
      if (message.CreatedUser == this.userName) {
        style = 'commentContentHighlight';
      }
      else {
        style = 'commentContent';
      }
      return style;
    },

    replyClass(thumbsUp) {
      var index = thumbsUp.find(item => item.CreatedUser == this.userName);
      if (index != null) {
        return 'fa fa-comment-o fa-2x commentActionStyle';
      }
      else {
        return 'fa fa-comment-o fa-2x commentActionStyle';
      }
    },

    addThumps(type, boardItemId, thumpType) {
      this.axios({
        method: 'post',
        url: '/Comment',
        data: {
          BoardItemId: boardItemId,
          Type: thumpType,
          CreatedUser: this.userName
        }
      }).then(() => {
        var comment = {
          CreatedUser: this.userName,
          Type: thumpType,
          BoardItemId: boardItemId
        };
        var context = {
          Operation: AddOperation,
          BoardId: this.boardId,
          Type: type,
          Comment: comment
        };
        this.sendCommentMsg(context);
      })
    },
    deleteThumps(type, boardItemId) {
      this.axios({
        method: 'delete',
        url: '/Comment',
        params: {
          borderItemId: boardItemId,
          username: this.userName
        }
      }).then(() => {
        var comment = {
          CreatedUser: this.userName,
          Type: 0,
          BoardItemId: boardItemId
        };
        var context = {
          Operation: DeleteOperation,
          BoardId: this.boardId,
          Type: type,
          Comment: comment
        };
        this.sendCommentMsg(context);
      })
    },

    deleteCommentItem(message) {
      this.$confirm(
        {
          message: 'Are you sure delete comment [' + message.Detail + '] ?',
          button: {
            no: 'No',
            yes: 'Yes'
          },
          callback: confirm => {
            if (confirm) {
              this.axios.delete('/Comment/' + message.Id + '')
                .then(() => {
                  var context = {
                    Operation: DeleteOperation,
                    BoardId: this.boardId,
                    Type: this.boardItemType,
                    Comment: message
                  };
                  this.sendCommentMsg(context);
                })
            }
          }
        })
    },
    addWellUp(wellItem) {
      this.thumbsUpAction(this.boardItemType, this.listOfItems, wellItem, CommentThumbupType);
    },

    thumbsUpAction(type, listOfItems, item, commentType) {
      let username = this.userName;
      var improveItemCache = listOfItems.find(c => c.Id == item.Id);
      var listThrumps = improveItemCache.ThumbsUp;
      var isAdd = true;

      var listItem = listThrumps.find(th => th.CreatedUser == username);
      if (listItem == null) {
        var likeitem = { BoardItemId: item.Id, CreatedUser: username, Id: 0, DateCreated: null, DateModified: null, Type: commentType };
        listThrumps.push(likeitem);
      }
      else {
        var index = listThrumps.findIndex(item => {
          if (item.CreatedUser == username) {
            return true;
          }
        })
        listThrumps.splice(index, 1);
        isAdd = false;
      }

      if (isAdd) {
        this.addThumps(type, item.Id, commentType);
      }
      else {
        this.deleteThumps(type, item.Id);
      }
    },
    toggleComment(boardItem) {
      boardItem.ToggleComment = !boardItem.ToggleComment;
    },
    renderFunc(message) {
      this.$Notice.success({
        //title: 'Notification',
        desc: 'The desc will hide when you set render.',
        render: h => {

          return h('span', [
            message
          ])

          //return h('span', [
          //    'This is created by ',
          //    h('a', 'render'),
          //    ' function'
          //])
        }
      });
    },
  }
}