const signalR = require("@microsoft/signalr");

export default {

  data() {
    return {
      UserToken: null,
      allBoardData: null,
      showBoardData: null,
      UserName: null,
      connection: "",
      pageSetting: {
        currentPage: 1,
        pageSize: 20,
        total:50
      }
    };
  },
  created() {

    // fetch the data when the view is created and the data is
    // already being observed
    this.UserToken = localStorage.getItem('TOKEN');
    this.UserName = localStorage.getItem('LOGINUSER');

    let boardDetailPath = sessionStorage.getItem("boardDetailPath");

    if (boardDetailPath != '' && boardDetailPath != null) {
      let board = JSON.parse(boardDetailPath);
      this.$router.push({ name: 'boardDetail', params: { boardId: board.boardId, boardName: board.boardName, createdUser: board.createdUser, state: board.state } });
    }
    else {
      this.fetchData();
      this.init();
    }

  },
  destroyed() {
    if (this.connection != null) {
      console.log("Hub for BoardAll with " + this.connection.connectionId + "is stopped");
      this.connection.stop();
    }
  },
  methods: {
    fetchData() {
      var msg = this.$Message.loading({
        content: 'Loading Boards...',
        duration: 0
      });

      var type = this.$route.params.type;
      var url = '/Board';
      if (type != null && type != 'undefined') {
        url = url + '/' + type + '';
      } else {
        url = url + '/0';
      }

      this.axios(
        {
          method: 'get',
          url: url
        })
        .then(json => {
          this.allBoardData = json.data;
          this.pageSetting.total = this.allBoardData.length;
          this.showBoardData = this.allBoardData.slice(
            (this.pageSetting.currentPage - 1) * this.pageSetting.pageSize,
            this.pageSetting.currentPage * this.pageSetting.pageSize);

          setTimeout(msg);
          return;
        }).catch(error => {
          setTimeout(msg);
          console.log('Failed to get board. Error:' + error);
        })
    },
    AddBoard() {
      this.$router.push('/addboard');
    },
    ViewBoard(board) {
      this.$router.replace({ name: 'boardDetail', params: { boardId: board.Id, boardName: board.Name, createdUser: board.CreatedUser, state: board.State } });
    },
    DeleteBoard(board) {
      this.$confirm(
        {
          message: 'Are you sure delete board [' + board.Name + '] ?',
          button: {
            no: 'No',
            yes: 'Yes'
          },
          callback: confirm => {
            if (confirm) {
              this.axios.delete('/Board/' + board.Id + '').then(() => {
                this.fetchData();
              }).then(() => {
                this.renderFunc(board.Name + ' is deleted successfully.');
              }).then(() => {
                this.sendBoardMessage();
              })
            }
          }
        }
      )
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
        // title: 'Notification',
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
      this.connection.on("ReceiveBoardMessage", () => {
        this.fetchData();
      });
      this.connection.start();
    },
    sendBoardMessage() {
      this.connection.invoke("SendBoardMessage");
    },

    pIndexChange(currentIndex) {
      this.pageSetting.currentPage = currentIndex;
      this.showBoardData = this.allBoardData.slice(
        (this.pageSetting.currentPage - 1) * this.pageSetting.pageSize,
        this.pageSetting.currentPage * this.pageSetting.pageSize);
    },

    pSizeChange(size) {
      this.pageSetting.pageSize = size;
      this.showBoardData = this.allBoardData.slice(
        (this.pageSetting.currentPage - 1) * this.pageSetting.pageSize,
        this.pageSetting.currentPage * this.pageSetting.pageSize);
    },

    formatBoardCreateTime(board) {
      return board.DateCreated.substring(0, 10);
		},
    getBoardCardFullBG(board) {

      if (board.IsDeleted == false && board.State == 1) {
        return 'rgb(238,255,242)';
      }
      return '#FFFFFF';
    },

    getBoardCardBG(board) {

      if (board.IsDeleted == false && board.State == 1) {
        return 'rgb(79 201 13)';
      }
      // else if(board.State == 2)
      // {
      //     return '#7FEE7A';
      // }
      else if (board.IsDeleted) {
        return '#f3c1c1';
      }
      return '#c4ebaf';
    },
    getBoardCardTooltip(board) {

      return board.Name + ' (Owner: ' + board.CreatedUser.toUpperCase() + ')' + '\n' +
        board.DateCreated;
    }
  },
}