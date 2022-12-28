const signalR = require("@microsoft/signalr");
const WentWellType = 1;
const NeedsImproveType = 2;
const ActionType = 3;

const CommentThumbupType = 0;
const CommentMessageType = 1;

const AddOperation = 'add';
const UpdateOperation = 'update';
const DeleteOperation = 'delete';
const SortUpOption = 'asc';
const SortDownOption = 'desc';
const SortCreatedOption = 'createdTime';
const SortUserOption = "createdUser";

const DefaultSortButtonClass = 'fa fa-sort fa-1x';
const DefaultSortButtonTitle = 'Sort Items';

export default {
  data() {
    return {
      loading: false,
      UserToken: null,
      userName: null,
      ActionContent: null,
      WellContent: null,
      ImproveContent: null,
      boardDetail: {
        WellDetail: "",
        ImproveDetail: "",
        ActionDetail: "",
      },
      boardItemTextChanged: false,
      commentItemTextChanged: false,
      boardName: null,
      boardId: null,
      boardCreatedUser: null,
      connection: "",
      state: 0,
      sortOption: SortUpOption,
      sortButtonClass: DefaultSortButtonClass,
      sortButtonTitle: DefaultSortButtonTitle,
      csvData: [],
      csvColumns: [],
      participants: [],
      showParticipants: true,

      fullscreen: false,
      teleport: true,
      pageOnly: true,
      
      currentFocusImproveItemId: null,
      actionItemBlockOffset: 0
    };
  },
  created() {
    console.log(this.$route.params);
    this.boardId = this.$route.params.boardId;
    this.boardName = this.$route.params.boardName;
    this.boardCreatedUser = this.$route.params.createdUser;
    this.state = this.$route.params.state;

    this.UserToken = localStorage.getItem('TOKEN');
    this.userName = localStorage.getItem('LOGINUSER').toUpperCase();
    sessionStorage.setItem('boardDetailPath', JSON.stringify(this.$route.params));
    this.fetchData(true);
    this.init();
    
    console.log(this.boardCreatedUser + this.userName);
  },
  destroyed() {
    if (this.connection != null) {

      this.updateParticipant(DeleteOperation);

      console.log("Hub for Board Detail with " + this.connection.connectionId + "is stopped");
      this.connection.stop();
    }
  },
  methods: {
    fetchData(forceRefresh) {
      this.fetchBoardItems(forceRefresh);
      this.fetchParticipants();
    },

    fetchBoardItems(forceRefresh) {

      var msg;
      if (forceRefresh) {
        msg = this.$Message.loading({
          content: 'Loading Board Items...',
          duration: 0
        });
      }

      this.axios(
        {
          method: 'get',
          url: '/BoardItem/' + this.boardId + ''
        })
        .then(all => {
          this.WellContent = all.data.filter(item => item.Type == WentWellType);
          this.ImproveContent = all.data.filter(item => item.Type == NeedsImproveType);
          this.ActionContent = all.data.filter(item => item.Type == ActionType);

          if (forceRefresh) {
            setTimeout(msg);
          }
          return;

        }).catch(error => {

          if (forceRefresh) {
            setTimeout(msg);
          }
          console.log('Failed to get board Items. Error:' + error);
        })
    },

    fetchParticipants() {
      this.axios({
        method: 'get',
        url: '/User/Online/' + this.boardId
      }).then((res) => {
        this.participants = [];
        res.data.Users.forEach((item) => {
          this.participants.push(item.Name);
        });
      }).catch(error => {
        this.$Message.error('Failed to load online users. Error:' + error);
      });
    },
    toggleOnlineUsers() {
      if (this.showParticipants) {
        this.showParticipants = false;
      }
      else {
        this.showParticipants = true;
      }
    },
    updateParticipant(action) {

      var context = {
        Operation: action,
        UserName: this.userName,
        BoardId: this.boardId
      };

      this.sendUserMsg(context);

      var opFlag = action == AddOperation ? 1 : 2;
      this.axios({
        method: 'post',
        url: '/User/Online',
        data: {
          Operation: opFlag,
          BoardId: this.boardId,
          Name: this.userName,
        }
      }).then(() => {

      }).catch(error => {
        console.log('Error:' + error);
      });
    },

    updateBoardNameKeydown(event) {
      console.log("test " + event.keyCode);
      if (event.keyCode == 13) {
        this.updateBoardName();
        event.preventDefault();
      }
    },
    updateBoardName() {
      if (this.$refs.editBoardName.innerText == this.boardName) {
        return;
      }
      this.axios({
        method: 'put',
        url: '/Board',
        data: {
          Id: this.boardId,
          Name: this.$refs.editBoardName.innerText
        }
      })
    },

    addBoardDetail(boardDetail, type, improvedItemId) {
      if (improvedItemId == null) {
        if (!boardDetail || !boardDetail.trim()) {
            return;
        }
      }

      if(this.loading)
      {
        return;
      }

      this.loading = true;

      this.axios({
        method: 'post',
        url: '/BoardItem',
        data: {
            boardid: this.boardId,
            detail: boardDetail,
            type: type,
            createduser: this.userName,
            associatedBoardItemId: improvedItemId
        }
      }).then((res) => {
        console.log("add");
        console.log(res.data);
        var listOfItems = this.getListOfItems(type);
        listOfItems.unshift(res.data);
        console.log(res.data);
        this.renderFunc(boardDetail + ' is created successfully.');

        var context = {
            Operation: AddOperation,
            BoardItem: res.data
        };
        this.sendBoardItemMsg(context);

        this.boardDetail.WellDetail = "";
        this.boardDetail.ImproveDetail = "";
        this.boardDetail.ActionDetail = "";

        this.loading = false;
      }).catch(error => {
        this.$Message.error('Failed to add board item. Error:' + error);
        this.loading = false;
      });
    },

    addWentWell() {
      this.addBoardDetail(this.boardDetail.WellDetail, WentWellType);
    },

    addImproved() {
      this.addBoardDetail(this.boardDetail.ImproveDetail, NeedsImproveType);
    },

    addAction() {
      this.addBoardDetail(this.boardDetail.ActionDetail, ActionType);
    },
    canEditBoardItem(boardItem) {
      if (this.state != 2 && boardItem.CreatedUser == this.userName) {
        return true;
      }

      return false;
    },
    canDeleteBoardItem(boardItem) {

      if (this.state != 2 && boardItem.CreatedUser == this.userName) {
        return true;
      }

      return false;
    },
    deleteBoardItem(boardItem) {

      this.$confirm(
        {
          message: 'Are you sure delete [' + boardItem.Detail + '] ?',
          button: {
            no: 'No',
            yes: 'Yes'
          },
          callback: confirm => {
            if (confirm) {

              var listOfItems = this.getListOfItems(boardItem.Type);
              this.removeBoardItemById(listOfItems, boardItem.Id);

              this.axios.delete('/BoardItem/' + boardItem.Id + '').then(() => {
                console.log(boardItem.Detail + 'is deleted');
                this.renderFunc(boardItem.Detail + ' is deleted successfully.');
              }).then(() => {
                var context = {
                  Operation: DeleteOperation,
                  BoardItem: boardItem
                };

                this.sendBoardItemMsg(context);
              })
            }
          }
        }
      )
    },

    getListOfItems(type) {
      var listOfItems;
      if (type == WentWellType) {
        listOfItems = this.WellContent;
      } else if (type == NeedsImproveType) {
        listOfItems = this.ImproveContent;
      } else if (type == ActionType) {
        listOfItems = this.ActionContent;
      }

      return listOfItems;
    },

    findIndexOfBoardItems(arr, id) {
      var index = -1;

      arr.find(function (item, i) {
        if (item.Id === id) {
          index = i;
          return i;
        }
      });

      return index;
    },

    findBoardItemById(arr, id) {
      var boardItem = null;
      arr.find(function (item) {
        if (item.Id === id) {
          boardItem = item;
          return item;
        }
      });

      return boardItem;
    },

    removeBoardItemById(arr, id) {
      var index = this.findIndexOfBoardItems(arr, id);
      if (index === -1) {
        return false;
      }
      return !!arr.splice(index, 1);
    },

    hasNoItemsInTheBoard() {
      return (this.ImproveContent == null || this.ImproveContent.length == 0) &&
        (this.WellContent == null || this.WellContent.length == 0) &&
        (this.ActionContent == null || this.ActionContent.length == 0)
    },
    boardItemChanged() {
      this.boardItemTextChanged = true;
    },

    updateBoardItem(boardItem) {

      if (!this.boardItemTextChanged) {
        return;
      }

      this.axios(
        {
          method: 'put',
          url: '/BoardItem',
          data: {
            id: boardItem.Id,
            detail: boardItem.Detail,
            type: boardItem.Type,
            createduser: boardItem.CreatedUser,
            boardid: this.boardId
          }
        }).then(() => {
          this.boardItemTextChanged = false;
        }).then(() => {
          this.renderFunc(boardItem.Detail + ' is updated successfully.');
        }).then(() => {

          var context = {
            Operation: UpdateOperation,
            BoardItem: boardItem
          };

          this.sendBoardItemMsg(context);
        })
    },

    markBoardItem(boardItem, state) {

      this.axios({
        method: 'put',
        url: '/BoardItem',
        data: {
          id: boardItem.Id,
          detail: boardItem.Detail,
          type: boardItem.Type,
          state: state,
          createduser: boardItem.CreatedUser,
          boardid: this.boardId
        }
      }).then(() => {
        boardItem.State = state;
        this.renderFunc(boardItem.Detail + ' is updated successfully.');
      }).then(() => {
        var context = {
          Operation: UpdateOperation,
          BoardItem: boardItem
        };
        this.sendBoardItemMsg(context);
      })
    },


    addWellUp(wellItem) {
      this.thumbsUpAction(WentWellType, this.WellContent, wellItem, CommentThumbupType);
    },

    addImproveUp(improveItem) {
      this.thumbsUpAction(NeedsImproveType, this.ImproveContent, improveItem, CommentThumbupType);
    },

    addActionUp(actionItem) {
      this.thumbsUpAction(ActionType, this.ActionContent, actionItem, CommentThumbupType);
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

    addWellComment(wellItem) {
      this.addComment(WentWellType, this.WellContent, wellItem, CommentMessageType);
    },
    addImproveComment(improveItem) {
      this.addComment(NeedsImproveType, this.ImproveContent, improveItem, CommentMessageType);
    },
    addActionComment(actionItem) {
      this.addComment(ActionType, this.ActionContent, actionItem, CommentMessageType);
    },

    toggleComment(boardItem) {
      boardItem.ToggleComment = !boardItem.ToggleComment;
    },

    addComment(type, listOfItems, boardItem, thumpType) {

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
          DateCreated: (new Date()).getTime() - 8*60*60*1000,
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

    commentItemChanged() {
      this.commentItemTextChanged = true;
    },

    updateWellCommentItem(message) {
      this.updateCommentItem(WentWellType, message);
    },
    updateImproveCommentItem(message) {
      this.updateCommentItem(NeedsImproveType, message);
    },
    updateActionCommentItem(message) {
      this.updateCommentItem(ActionType, message);
    },

    updateCommentItem(type, comment) {

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
            Type: type,
            Comment: comment
          };
          this.sendCommentMsg(context);
        });
    },
    canEditComment(comment) {
      if (comment.CreatedUser == this.userName) {
        return true;
      }

      return false;
    },

    commentAction(type, listOfItems, item, commentType) {
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

    getUserAvatar(userName) {
      let userAvatar = ""
      try {
        userAvatar = require('../assets/Person/' + userName.toLowerCase() + '.jpg')
        return userAvatar
      } catch (e) {
        userAvatar = require('../assets/Person/cube.jpg')
        return userAvatar
      }
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

    init() {
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl("http://techgroupdockerdc:5050/BoardHub", {})
        .configureLogging(signalR.LogLevel.Error)
        .build();
      this.connection.on("ReceiveBoardItemMessage", boardItemEvent => {
        if (boardItemEvent.BoardItem.BoardId == this.boardId) {
          var listOfItems = this.getListOfItems(boardItemEvent.BoardItem.Type);

          if (boardItemEvent.Operation == DeleteOperation) {
            this.removeBoardItemById(listOfItems, boardItemEvent.BoardItem.Id);
          }
          else if (boardItemEvent.Operation == AddOperation) {
            console.log('New');
            console.log(boardItemEvent.BoardItem);
            var index = this.findIndexOfBoardItems(listOfItems, boardItemEvent.BoardItem.Id);
            if (index == -1) {
              listOfItems.unshift(boardItemEvent.BoardItem);
            }
          }
          else if (boardItemEvent.Operation == UpdateOperation) {
            console.log(boardItemEvent.BoardItem);
            var boarditemToBeUpdated = this.findBoardItemById(listOfItems, boardItemEvent.BoardItem.Id);
            if (boarditemToBeUpdated != null) {
              boarditemToBeUpdated.Detail = boardItemEvent.BoardItem.Detail;
              boarditemToBeUpdated.State = boardItemEvent.BoardItem.State;
            }
          }
          else {
            this.fetchData(false);
          }
        }
      });

      this.connection.on("ReceiveCommentMessage", commentEvent => {
        if (commentEvent.BoardId == this.boardId) {
          var listOfItems = this.getListOfItems(commentEvent.Type);

          if (commentEvent.Operation == AddOperation) {
            var improveItemCache1 = listOfItems.find(c => c.Id == commentEvent.Comment.BoardItemId);

            if (commentEvent.Comment.Type == CommentThumbupType) {
              var listThumbups = improveItemCache1.ThumbsUp;

              var listItem = listThumbups.find(th => th.CreatedUser == commentEvent.Comment.CreatedUser);
              if (listItem == null) {
                listThumbups.push(commentEvent.Comment);
              }
            }
            else {
              improveItemCache1.Messages.push(commentEvent.Comment);
            }
          }
          else if (commentEvent.Operation == DeleteOperation) {

            var improveItemCache2 = listOfItems.find(c => c.Id == commentEvent.Comment.BoardItemId);

            if (commentEvent.Comment.Type == CommentThumbupType) {
              var listThrumps2 = improveItemCache2.ThumbsUp;
              var index = listThrumps2.findIndex(item => {
                if (item.CreatedUser == commentEvent.Comment.CreatedUser) {
                  return true;
                }
              });
              if (index == -1) {
                return;
              }
              listThrumps2.splice(index, 1);
            }
            else {
              var listComments = improveItemCache2.Messages;
              var index2 = listComments.findIndex(item => {
                if (item.Id == commentEvent.Comment.Id) {
                  return true;
                }
              });
              if (index2 == -1) {
                return;
              }

              listComments.splice(index2, 1);
            }
          }
          else if (commentEvent.Operation == UpdateOperation) {
            var improveItemCache3 = listOfItems.find(c => c.Id == commentEvent.Comment.BoardItemId);
            if (commentEvent.Comment.Type == CommentMessageType) {
              var commentToBeUpdated = improveItemCache3.Messages.find(m => m.Id == commentEvent.Comment.Id);
              if (commentToBeUpdated != null) {
                commentToBeUpdated.Detail = commentEvent.Comment.Detail;
              }
            }
          }
        }
      });

      this.connection.on("ReceiveUserMessage", userEvent => {
        if (userEvent.BoardId == this.boardId) {
          if (userEvent.UserName != this.userName) {
            if (userEvent.Operation == AddOperation) {
              this.$Notice.info({
                render: h => {
                  return h('span', [
                    userEvent.UserName + ' joined the board.'
                  ])
                }
              });

              var index = this.participants.indexOf(userEvent.UserName);
              if (index == -1) {
                this.participants.push(userEvent.UserName);
              }
            }
            else if (userEvent.Operation == DeleteOperation) {
              this.$Notice.info({
                render: h => {
                  return h('span', [
                    userEvent.UserName + ' left the board.'
                  ])
                }
              });

              var index2 = this.participants.findIndex(item => {
                if (item == userEvent.UserName) {
                  return true;
                }
              });
              if (index2 == -1) {
                return;
              }
              this.participants.splice(index2, 1);
            }
          }
        }
      });

      this.connection.start().then(() => {
        var index = this.participants.indexOf(this.userName);
        if (index == -1) {
          this.participants.push(this.userName);
        }

        this.updateParticipant(AddOperation);
      });
    },

    sendBoardMsg() {
      this.connection.invoke("SendBoardMessage");
    },

    sendBoardItemMsg(context) {
      this.connection.invoke("SendBoardItemMessage", context);
    },

    sendCommentMsg(context) {
      this.connection.invoke("SendCommentMessage", context);
    },

    sendUserMsg(context) {
      this.connection.invoke("SendUserMessage", context);
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

    canDeleteComment(message) {

      if (message.CreatedUser == this.userName) {
        return true;
      }

      return false;
    },

    deleteWellComment(message) {
      this.deleteComment(WentWellType, message);
    },

    deleteImproveComment(message) {
      this.deleteComment(NeedsImproveType, message);
    },

    deleteActionComment(message) {
      this.deleteComment(ActionType, message);
    },
    deleteComment(type, message) {
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
                    Type: type,
                    Comment: message
                  };
                  this.sendCommentMsg(context);
                })
            }
          }
        })
    },
    markCompleted() {
      this.$confirm(
        {
          message: 'Are you sure mark board [' + this.boardName + "] as Completed ? after that, you can't edit this board again",
          button: {
            no: 'No',
            yes: 'Yes'
          },
          callback: confirm => {
            if (confirm) {
              this.axios({
                method: 'put',
                url: '/Board',
                data: {
                  id: this.boardId,
                  name: this.boardName,
                  state: 2,
                  createduser: this.userName
                }
              }).then(() => {
                this.state = 2;
                this.renderFunc(this.boardName + ' is marked as completed successfully.');
              }).then(() => {
                this.sendBoardItemMsg();
              })
            }
          }
        }
      )
    },

    deleteBoard() {
      this.$confirm(
        {
          message: 'Are you sure delete board [' + this.boardName + '] ?',
          button: {
            no: 'No',
            yes: 'Yes'
          },
          callback: confirm => {
            if (confirm) {
              this.axios.delete('/Board/' + this.boardId + '').then(() => {
                this.renderFunc(this.boardName + ' is deleted successfully.');
              }).then(() => {
                this.sendBoardMsg();
              }).then(() => {
                this.$router.push('/boardAll');
              })
            }
          }
        }
      )
    },
    exportData() {
      this.csvColumns = [
        {
          "title": "",
          "key": "content",
        },
        {
          "title": "",
          "key": "createdUser",
        },
        {
          "title": "",
          "key": "vote",
        }];

      this.csvData.push({ "content": this.boardName, "createdUser": "", "vote": "" });
      this.csvData.push({ "content": "", "createdUser": "", "vote": "" });
      this.csvData.push({ "content": "Went Well", "createdUser": "Submitter", "vote": "Votes" });
      this.getSortedItems(this.WellContent).forEach((value, index) => {
        this.csvData.push({ "content": index + 1 + '.' + value.content, "createdUser": value.createdUser, "vote": value.vote });
      });

      this.csvData.push({ "content": "", "createdUser": "", "vote": "" });
      this.csvData.push({ "content": "To Improve", "createdUser": "Submitter", "vote": "Votes" });
      this.getSortedItems(this.ImproveContent).forEach((value, index) => {
        this.csvData.push({ "content": index + 1 + '.' + value.content, "createdUser": value.createdUser, "vote": value.vote });
      });

      this.csvData.push({ "content": "", "createdUser": "", "vote": "" });
      this.csvData.push({ "content": "Action Items", "createdUser": "Submitter", "vote": "Votes" });
      this.getSortedItems(this.ActionContent).forEach((value, index) => {
        this.csvData.push({ "content": index + 1 + '.' + value.content, "createdUser": value.createdUser, "vote": value.vote });
      });

      this.$refs.tableForExport.exportCsv({
        filename: this.boardName,
        columns: this.csvColumns,
        data: this.csvData
      })
    },
    getSortedItems(content) {
      let sortedItems = [];
      content.forEach((value) => {
        sortedItems.push({
          "content": value.Detail.replaceAll(',', '_').replaceAll('\n', '').replaceAll('\r', ''),
          "createdUser": value.CreatedUser,
          "vote": value.ThumbsUp.length
        });
      });

      sortedItems.sort(function (a, b) {
        return b.vote - a.vote
      });

      return sortedItems;
    },
    sortItems() {
      if (this.sortOption == SortUpOption) {
        this.sortItemsAsc(this.WellContent);
        this.sortItemsAsc(this.ImproveContent);
        this.sortItemsAsc(this.ActionContent);
        this.sortButtonClass = "fa fa-sort-amount-desc fa-1x";
        this.sortButtonTitle = "Sorted Items by Thumbup Descending";

        this.sortOption = SortDownOption;
      }
      else if (this.sortOption == SortDownOption) {
        this.sortItemsDesc(this.WellContent);
        this.sortItemsDesc(this.ImproveContent);
        this.sortItemsDesc(this.ActionContent);
        this.sortButtonClass = "fa fa-sort-amount-asc fa-1x";
        this.sortButtonTitle = "Sorted Items by Thumbup Ascending";

        this.sortOption = SortUserOption;
      }
      else if (this.sortOption == SortUserOption) {
        this.sortItemsUserAsc(this.WellContent);
        this.sortItemsUserAsc(this.ImproveContent);
        this.sortItemsUserAsc(this.ActionContent);
        this.sortButtonClass = "fa fa-sort-alpha-asc fa-1x";
        this.sortButtonTitle = "Sorted Items by User Name Ascending";

        this.sortOption = SortCreatedOption;

      }
      else if (this.sortOption == SortCreatedOption) {
        this.resetSortItems(this.WellContent);
        this.resetSortItems(this.ImproveContent);
        this.resetSortItems(this.ActionContent);
        this.sortButtonClass = DefaultSortButtonClass;
        this.sortButtonTitle = DefaultSortButtonTitle;

        this.sortOption = SortUpOption;
      }
    },
    sortItemsAsc(content) {
      content.sort(function (a, b) {
        return b.ThumbsUp.length - a.ThumbsUp.length;
      });
    },
    sortItemsDesc(content) {
      content.sort(function (a, b) {
        return a.ThumbsUp.length - b.ThumbsUp.length;
      });
    },
    sortItemsUserAsc(content) {
      content.sort(function (a, b) {
        return a.CreatedUser.localeCompare(b.CreatedUser);
      });
    },
    resetSortItems(content) {
      content.sort(function (a, b) {
        return b.Id - a.Id;
      });
    },
    getBoardItemClass(type, state) {
      var style = 'boardItemContent';
      if (state == 2) {
        return style + ' ' + 'boardItemDoneClass';
      }

      if (type == NeedsImproveType) {
        return style + ' ' + 'improveItem';
      }
      else if (type == ActionType) {
        return style + ' ' + 'actionItem';
      }
    },
    toggle() {
      this.fullscreen = !this.fullscreen
    },
    handleKeydown(event) {
      if (event.shiftKey && event.keyCode == 13) {
        event.preventDefault();
        if (event.target.id == 'wentWell') {
          this.boardDetail.WellDetail += '\n';
        }

        if (event.target.id == 'improved') {
          this.boardDetail.ImproveDetail += '\n';
        }

        if (event.target.id == 'action') {
          this.boardDetail.ActionDetail += '\n';
        }
      }
      else if (event.keyCode == 13) {
        event.preventDefault();
        if (event.target.id == 'wentWell') {
          this.addWentWell();
        }

        if (event.target.id == 'improved') {
          this.addImproved();
        }

        if (event.target.id == 'action') {
          this.addAction();
        }
      }
    },
    clickContent(event) {
      console.log(event);
    },
    addAssociatedActionItem(improvedItemId) {
      this.addBoardDetail(this.boardDetail.ActionDetail, ActionType, improvedItemId);
    },
    showAllAssociatedActions(improvedItemId) {
      if (improvedItemId == this.currentFocusImproveItemId) {
        this.blurImproveItem();
      }
      else {
        this.focusImproveItem(improvedItemId);
      }
    },
    focusAssociatedImproveItem(associatedBoardItemId) {
      if (this.currentFocusImproveItemId == null && associatedBoardItemId != null) {
        this.focusImproveItem(associatedBoardItemId);
      }
    },
    refocusImproveItem(improvedItemId) {
      if (improvedItemId != this.currentFocusImproveItemId && this.currentFocusImproveItemId != null) {
        this.blurImproveItem();
        this.focusImproveItem(improvedItemId);
      }
    },
    cleanFocusedImproveItem(improvedItemId) {
      if (improvedItemId != null) {
        this.blurImproveItem();
      }
    },
    focusImproveItem(improvedItemId) {
      this.currentFocusImproveItemId = improvedItemId;
      this.calculateOffset(improvedItemId);
    },
    blurImproveItem() {
      this.currentFocusImproveItemId = null;
      this.actionItemBlockOffset = 0;
    },
    calculateOffset(improvedItemId) {
      this.$refs.improvedItemCard.some((item) => {
        if (item.$attrs.id == improvedItemId) {
          this.actionItemBlockOffset = item.$el.getBoundingClientRect().top - this.$refs.actionItemBlock.getBoundingClientRect().top;
          return true;
        }
      });
    },
    setImproveItemOpacity(improvedItemId) {
      return this.currentFocusImproveItemId == improvedItemId || this.currentFocusImproveItemId == null? '1.0' : '0.2';
    },
    getViewActionsClass(improvedItemId) {
      var css = "fa fa-2x ";
      if (this.currentFocusImproveItemId == improvedItemId) {
          css +=  " fa-hand-o-right ";
          return css + 'viewActionHighlightStyle';
      }
      else {
          css += " fa-hand-o-right ";
          return css + 'viewActionStyle';
      }
    }
  }
}