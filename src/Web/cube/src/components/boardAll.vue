<template>
    <div style="margin:0">
        <ul>
            <li style="width:260px; float: left;">
                <Card style="width: 250px; cursor: pointer;" v-on:click.native="AddBoard()">
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
                <Card style="width: 250px; text-align: left; cursor: pointer">
                    <p slot="title">
                        <img :src="getUserAvatar(board.CreatedUser)" :title="board.CreatedUser" style="width:20px; height:20px; border-radius:50%; " />
                    </p>
                    <div style="text-align:center;">
                        <a href="#" slot="extra" @click.prevent="ViewBoard(board)">
                            <Icon type="ios-loop-strong"></Icon>
                            {{board.Name}}
                        </a>
                    </div>
                    <a href="#" slot="extra" @click.prevent="DeleteBoard(board)" title="Delete" v-if="board.CreatedUser==UserName">
                        <span aria-label="Delete" class="">
                            <button class="css-b7766g" tabindex="-1" type="button" aria-label="Delete" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 42px;">
                                <svg class="css-vubbuv" focusable="false" viewBox="0 0 24 24" aria-hidden="true" style="color: rgb(239, 83, 80);">
                                    <use xlink:href="#at-delete"></use>
                                </svg>
                            </button>
                        </span>
                    </a>
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
            this.fetchData();
            this.UserToken = localStorage.getItem('TOKEN');
            this.UserName = localStorage.getItem('LOGINUSER');
            
            this.init();
        },
        methods: {
            fetchData() {

                var msg = this.$Message.loading({
                        content: 'Loading...',
                        duration: 0
                    });

                fetch('Board')
                    .then(r => r.json())
                    .then(json => {
                        
                        this.post = json;
                        this.pageSetting.total = this.post.length;

                        console.log(json);
                        console.log(this.pageSetting);

                        setTimeout(msg);

                        return;
                    });
            },
            AddBoard() {
                this.$router.push('/addboard');
            },
            ViewBoard(board) {
                this.$router.push({ name: 'boardDetail', params: { boardId: board.Id, boardName: board.Name, state:board.State } });
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
                                this.axios.delete('/Board/' + board.Id + '',
                                    {
                                        headers: {
                                            'Authorization': 'Bearer ' + this.UserToken
                                        }
                                    }).then(() => {
                                        this.fetchData();
                                    }).then(() => {
                                        this.renderFunc(board.Name + ' is deleted successfully.');
                                    }).then(() => {
                                        this.sendMsg();
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
                    title: 'Notification',
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
            sendMsg() {
                this.connection.invoke("SendBoardMessage");
            },
            pageChanged(page) {
                console.log(page);
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
        background: '#fff'
    }

</style>