<template>
    <div class="subLayout">
        <ul>
            <li style="width:260px; float: left;">
                <Card style="width: 250px; cursor: pointer; margin:0px 0px 15px 0px" v-on:click.native="AddBoard()">
                    <div style="display: flex; padding-bottom: 36px; background: #fff;">
                        <i scripturl="../scripts/font.js" style="font-size:24px;">
                            <svg viewBox="0 0 1024 1024" style="width:1em;height:1em;fill:currentColor;overflow:hidden">
                                <use xlink:href="#at-plus"></use>
                            </svg>
                        </i>
                    </div>
                    <span style="position:absolute;left:16px;bottom:12px;">Add Board</span>
                </Card>
            </li>
            <li v-for="board in post" :key="board.Id" style="width:260px; float: left;">
                <Card :style="{width: '250px', cursor: 'pointer', margin:'0px 0px 15px 0px', background: getBoardCardBG(board)}" v-on:click.native="ViewBoard(board)">
                    <p slot="title" :title="getBoardCardTooltip(board)">
                        <span style="float:right">
                            <img :src="getUserAvatar(board.CreatedUser)" :title="'Owner: ' + board.CreatedUser" style="width:20px; height:20px; border-radius:50%; " />
                        </span>
                       <span>
                            <img v-if="board.State == 2" src="../assets/Icons/completed.jpg" title="Completed" style="width:20px; height:20px; border-radius:50%;" >           
                            <img v-if="board.IsDeleted==false && board.State == 1" src="../assets/Icons/inprogress.jpg" title="In Progress" style="width:20px; height:20px; border-radius:50%; " >
                            <img v-if="board.IsDeleted" src="../assets/Icons/deleted.png" title="Deleted" style="width:20px; height:20px; border-radius:50%;" >
                        </span>
                    </p>
                    <div style="text-align: center; overflow: hidden; white-space: nowrap; text-overflow: ellipsis;">
                        <a href="#" slot="extra" :title="getBoardCardTooltip(board)">
                            {{board.Name}}
                        </a>
                    </div>
                    <!-- <a href="#" slot="extra" @click.prevent="DeleteBoard(board)" title="Delete" v-if="board.CreatedUser==UserName">
                        <span aria-label="Delete" class="">
                            <button class="css-b7766g" tabindex="-1" type="button" aria-label="Delete" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 42px;">
                                <svg class="css-vubbuv" focusable="false" viewBox="0 0 24 24" aria-hidden="true" style="color: rgb(239, 83, 80);">
                                    <use xlink:href="#at-delete"></use>
                                </svg>
                            </button>
                        </span>
                    </a> -->
                </Card>
            </li>
        </ul>
        <div class="page-footer">
            <Page :total="pageSetting.total" :page-size="pageSetting.pageSize" :current="pageSetting.currentPage" @on-change="pageChanged" />
        </div>
    </div>
</template>

<script>
    const signalR = require("@microsoft/signalr");

    export default {

        data() {
            return {
                UserToken: null,
                post: null,
                UserName:null,
                connection: "",
                pageSetting: {
                    currentPage: 1,
                    pageSize: 50,
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
        destroyed(){
            if(this.connection!=null){
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
                if(type!=null && type != 'undefined')
                {
                    url = url + '/' + type + '';
                }else{
                    url = url + '/0';
                }
                
                this.axios(
                    {
                        method: 'get',
                        url: url
                    })
                    .then(json => {
                        this.post = json.data;
                        this.pageSetting.total = this.post.length;

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
                this.$router.replace({name: 'boardDetail', params: { boardId: board.Id, boardName: board.Name, createdUser:board.CreatedUser, state:board.State } });
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
                    .withUrl("http://10.63.224.86:5050/BoardHub", {})
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
            pageChanged(page) {
                console.log(page);
            },

            getBoardCardBG(board) {

                if(board.IsDeleted==false && board.State == 1)
                {
                    return '#F3FCF1';
                }
                // else if(board.State == 2)
                // {
                //     return '#7FEE7A';
                // }
                else if(board.IsDeleted)
                {
                    return '#FCE9E9';
                }

                return '#FFF';
            },
            getBoardCardTooltip(board) {

                return board.Name + ' (Owner: ' + board.CreatedUser.toUpperCase() + ')' + '\n' + 
                       board.DateCreated;
            }
        },
    }
</script>

<style>
    .page-footer {
        position: absolute;
        bottom: 0;
        left: 50%;
        transform: translate(-50%, -50%);
        background: #fff
    }
    .subLayout{
        margin: 0;
        padding: 12px;
    }

    .subLayout .ivu-card-body {
        padding: 16px;
    }
</style>