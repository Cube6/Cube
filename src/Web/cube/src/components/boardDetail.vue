<template>
    <div>
        <h1 style="width:100%;text-align:center; font-size:larger">
            <span>

                {{boardName}}

                <span v-if="state == 2">Completed</span>
                <Dropdown v-if="state != 2" style="float: right;position: relative; font-size:12pt; ">
                    <Icon type="ios-more" size="28"></Icon>
                    <DropdownMenu slot="list">
                        <DropdownItem v-on:click.native="markCompleted()">Mark as Completed</DropdownItem>
                        <DropdownItem v-on:click.native="deleteBoard()">Delete</DropdownItem>
                    </DropdownMenu>
                </Dropdown>
            </span>
        </h1>
        <br />
        <table width="100%">
            <thead>
                <tr>
                    <th width="33%">
                        <Input v-model="boardDetail.WellDetail" placeholder="What went well ?" search enter-button="Add" @on-search="addWentWell" />
                    </th>
                    <th width="33%">
                        <Input v-model="boardDetail.ImporveDetail" placeholder="What could be improved ?" search enter-button="Add" @on-search="addImporved" />
                    </th>
                    <th width="34%">
                        <Input v-model="boardDetail.ActionDetail" placeholder="A brilliant idea to share ?" search enter-button="Add" @on-search="addAction" />
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td style="vertical-align:top">
                        <ul>
                            <li v-for="well in WellContent" :key="well.Id">
                                <Card style="width: 100%; text-align: left;">
                                    <img :src="getUserAvatar(well.CreatedUser)" :title="well.CreatedUser" style="float: right; width: 20px; height: 20px; border-radius: 50%; " />

                                    <Input v-model="well.Detail" class="boardItemContent" type="textarea" style="border-style: none" :autosize="true" @on-blur="updateBoardItem(well)" />
                                    <p style="height:22px;">
                                        <a href="#" @click.prevent="addWellUp(well)" title="Delete">
                                            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 64px;">
                                                <svg class="css-vubbuv" focusable="false" viewBox="0 0 24 24" aria-hidden="true" style="color: rgb(46, 125, 50);">
                                                    <use xlink:href="#at-handUp"></use>
                                                </svg>
                                                &nbsp;<p>{{well.ThumbsUp.length}}</p>
                                            </button>
                                        </a>
                                        <a href="#" @click.prevent="addWellDown(well)" title="Delete">
                                            <button class="css-b7766g" tabindex="-1" type="button" aria-label="Dislike" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 64px;">
                                                <svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeMedium css-vubbuv" focusable="false" viewBox="0 0 24 24" aria-hidden="true" data-testid="ThumbDownOutlinedIcon" style="color: rgb(239, 83, 80);">
                                                    <use xlink:href="#at-handDown"></use>
                                                </svg>
                                                &nbsp;<p>{{well.ThumbsDown.length}}</p>
                                            </button>
                                        </a>
                                        <a href="#" style="float:right;" @click.prevent="deleteBoardItem(well)" title="Delete">
                                            <span aria-label="Delete" class="">
                                                <button class="css-b7766g" tabindex="-1" type="button" aria-label="Delete" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 42px;">
                                                    <svg class="css-vubbuv" focusable="false" viewBox="0 0 24 24" aria-hidden="true" style="color: rgb(239, 83, 80);">
                                                        <use xlink:href="#at-delete"></use>
                                                    </svg>
                                                </button>
                                            </span>
                                        </a>
                                    </p>
                                </Card>
                            </li>
                        </ul>
                    </td>
                    <td style="vertical-align:top">
                        <ul>
                            <li v-for="imporve in ImporveContent" :key="imporve.Id">
                                <Card style="width: 100%; text-align: left;">
                                    <img :src="getUserAvatar(imporve.CreatedUser)" :title="imporve.CreatedUser" style="float: right; width: 20px; height: 20px; border-radius: 50%; " />

                                    <Input v-model="imporve.Detail" class="boardItemContent" type="textarea" :autosize="true" @on-blur="updateBoardItem(imporve)" />
                                    <p style="height:22px;">
                                        <a href="#" @click.prevent="addImproveUp(imporve)" title="Delete">
                                            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 64px;">
                                                <svg class="css-vubbuv" focusable="false" viewBox="0 0 24 24" aria-hidden="true" style="color: rgb(46, 125, 50);">
                                                    <use xlink:href="#at-handUp"></use>
                                                </svg>
                                                &nbsp;<p>{{imporve.ThumbsUp.length}}</p>
                                            </button>
                                        </a>
                                        <a href="#" @click.prevent="addImproveDown(imporve)" title="Delete">
                                            <button class="css-b7766g" tabindex="-1" type="button" aria-label="Dislike" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 64px;">
                                                <svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeMedium css-vubbuv" focusable="false" viewBox="0 0 24 24" aria-hidden="true" data-testid="ThumbDownOutlinedIcon" style="color: rgb(239, 83, 80);">
                                                    <use xlink:href="#at-handDown"></use>
                                                </svg>
                                                &nbsp;<p>{{imporve.ThumbsDown.length}}</p>
                                            </button>
                                        </a>
                                        <a href="#" style="float:right" @click.prevent="deleteBoardItem(imporve)" title="Delete">
                                            <span aria-label="Delete" class="">
                                                <button class="css-b7766g" tabindex="-1" type="button" aria-label="Delete" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 42px;">
                                                    <svg class="css-vubbuv" focusable="false" viewBox="0 0 24 24" aria-hidden="true" style="color: rgb(239, 83, 80);">
                                                        <use xlink:href="#at-delete"></use>
                                                    </svg>
                                                </button>
                                            </span>
                                        </a>
                                    </p>

                                </Card>
                            </li>
                        </ul>
                    </td>
                    <td style="vertical-align:top">
                        <ul>
                            <li v-for="action in ActionContent" :key="action.Id">
                                <Card style="width: 100%; text-align: left; ">
                                    <img :src="getUserAvatar(action.CreatedUser)" :title="action.CreatedUser" style="float: right; width: 20px; height: 20px; border-radius: 50%; " />

                                    <Input v-model="action.Detail" class="boardItemContent" type="textarea" :autosize="true" @on-blur="updateBoardItem(action)" />

                                    <p style="height:22px; ">
                                        <a href="#" @click.prevent="addActionUp(action)">
                                            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 64px;">
                                                <svg class="css-vubbuv" focusable="false" viewBox="0 0 24 24" aria-hidden="true" style="color: rgb(46, 125, 50);">
                                                    <use xlink:href="#at-handUp"></use>
                                                </svg>
                                                &nbsp;<p>{{action.ThumbsUp.length}}</p>
                                            </button>
                                        </a>
                                        <a href="#" @click.prevent="addActionDown(action)">
                                            <button class="css-b7766g" tabindex="-1" type="button" aria-label="Dislike" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 64px;">
                                                <svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeMedium css-vubbuv" focusable="false" viewBox="0 0 24 24" aria-hidden="true" data-testid="ThumbDownOutlinedIcon" style="color: rgb(239, 83, 80);">
                                                    <use xlink:href="#at-handDown"></use>
                                                </svg>
                                                &nbsp;<p>{{action.ThumbsDown.length}}</p>
                                            </button>
                                        </a>

                                        <a href="#" @click.prevent="deleteBoardItem(action)" title="Delete" style="float:right">
                                            <Button type="text" class="css-b7766g" tabindex="-1" aria-label="Delete" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 42px;">
                                                <svg class="css-vubbuv" focusable="false" viewBox="0 0 24 24" aria-hidden="true" style="color: rgb(239, 83, 80);">
                                                    <use xlink:href="#at-delete"></use>
                                                </svg>
                                            </Button>
                                        </a>
                                    </p>

                                </Card>
                            </li>
                        </ul>
                    </td>
                </tr>
            </tbody>
        </table>

        <BackTop :height="100" :bottom="120" :right="50">
            <div class="top">Back to Top</div>
        </BackTop>
    </div>
</template>

<script>
    const signalR = require("@microsoft/signalr");

    export default {
        data() {
            return {
                UserToken: null,
                userName: null,
                ActionContent: null,
                WellContent: null,
                ImporveContent: null,
                deleteActionId: null,
                deleteWellId: null,
                deleteImporveId: null,
                ActionUpCount: 0,
                ActionDownCount: 0,
                WellUpCount: 0,
                WellDownCount: 0,
                ImproveUpCount: 0,
                ImproveDownCount: 0,
                boardDetail: {
                    WellDetail: "",
                    ImporveDetail: "",
                    ActionDetail: "",
                },
                listThrumps: [
                    { userid: null,bid: null, upCount:0,downCount:0 }
                ],
                boardName:null,
                boardId: null,
                connection: "",
            };
        },
        created() {
            this.boardId = this.$route.params.boardId;
            this.boardName = this.$route.params.boardName;
            this.state = this.$route.params.state;
            console.log(this.state);
            this.fetchData();
            this.UserToken = localStorage.getItem('TOKEN');
            this.userName = localStorage.getItem('LOGINUSER').toUpperCase();

            this.init();
        },
 
        methods: {
            fetchData() {
                this.axios.get('/BoardItem/' + this.boardId + '')
                    .then(all => {
                        this.WellContent = all.data.filter(item => item.Type == 1);
                        this.ImporveContent = all.data.filter(item => item.Type == 2);
                        this.ActionContent = all.data.filter(item=>item.Type==3);
                    return;
                }).catch(error => {
                    console.log(error);
                })
            },
            addBoardDetail(boardDetail, type) {
                this.axios({
                    method: 'post',
                    url: '/BoardItem',
                    data: {
                        boardid: this.boardId,
                        detail: boardDetail,
                        type: type,
                        createduser: this.userName
                    },
                    headers: {
                        'Authorization': 'Bearer ' + this.UserToken
                    }
                }).then(() => {
                    this.fetchData();
                }).then(() => {
                    this.renderFunc(boardDetail + ' is created successfully.');
                }).then(() => {
                    this.sendMsg();
                    this.boardDetail.WellDetail = "";
                    this.boardDetail.ImporveDetail = "";
                    this.boardDetail.ActionDetail = "";
                })
            },
            addWentWell() {
                this.addBoardDetail(this.boardDetail.WellDetail, 1);
               
            },
            addImporved() {
                this.addBoardDetail(this.boardDetail.ImporveDetail, 2);
               
            },
            addAction() {
                this.addBoardDetail(this.boardDetail.ActionDetail, 3);
                
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
                                this.axios.delete('/BoardItem/' + boardItem.Id + '',
                                    {
                                        headers: {
                                            'Authorization': 'Bearer ' + this.UserToken
                                        }
                                    }).then(() => {
                                        this.fetchData();
                                    }).then(() => {
                                        this.renderFunc(boardItem.Detail + ' is deleted successfully.');
                                    }).then(() => {
                                        this.sendMsg();
                                    })
                            }
                        }
                    }
                )
            },
            updateBoardItem(boardItem) {
                console.log(boardItem.Id);
                this.axios({
                    method: 'put',
                    url: '/BoardItem',
                    data: {
                        id: boardItem.Id,
                        detail: boardItem.Detail,
                        type: boardItem.Type,
                        createduser: this.userName,
                        boardid: this.boardId
                    },
                    headers: {
                        'Authorization': 'Bearer ' + this.UserToken
                    }
                }).then(() => {
                    this.fetchData();
                }).then(() => {
                    this.renderFunc(boardItem.Detail + ' is updated successfully.');
                }).then(() => {
                    this.sendMsg();
                })
            },
            addActionUp(actionItem) {
                let username = this.userName;
                var listItem = this.listThrumps.find(item => item.bid == actionItem.Id && item.username == username);
                if (listItem == null) {
                    var likeitem = { bid: actionItem.Id, username: username, upCount: 1, downCount: 0 };
                    this.listThrumps.push(likeitem);
                    this.ActionUpCount = 1;
                    this.ActionDownCount = 0;
                }
                else {
                    if (listItem.upCount > 0) {
                        this.$Message.error('You already liked it !');
                    }
                    else {
                        listItem.upCount = 1;
                        listItem.downCount = 0;
                        this.ActionUpCount = 1;
                        this.ActionDownCount = 0;
                    }
                }

            },
            addActionDown(actionItem) {
                let username = this.userName;
                var listItem = this.listThrumps.find(item => item.bid == actionItem.Id && item.username == username);
                if (listItem == null) {
                    var likeitem = { bid: actionItem.Id, username: username, upCount: 1, downCount: 0 };
                    this.listThrumps.push(likeitem);
                }
                else {
                    if (listItem.downCount > 0) {
                        this.$Message.error('You already unliked it !');
                    }
                    else {
                        listItem.upCount = 0;
                        listItem.downCount = 1;
                        this.ActionUpCount = 0;
                        this.ActionDownCount = 1;
                    }
                }
            },
            addWellUp(wellItem) {
                let username = this.userName;
                var listItem = this.listThrumps.find(item => item.bid == wellItem.Id && item.username == username);
                if (listItem == null) {
                    var likeitem = { bid: wellItem.Id, username: username, upCount: 1, downCount: 0 };
                    this.listThrumps.push(likeitem);
                    this.WellUpCount = 1;
                    this.WellDownCount = 0;
                }
                else {
                    if (listItem.upCount > 0) {
                        this.$Message.error('You already liked it !');
                    }
                    else {
                        listItem.upCount = 1;
                        listItem.downCount = 0;
                        this.WellUpCount = 1;
                        this.WellDownCount = 0;
                    }
                }
            },
            addWellDown(wellItem) {
                let username = this.userName;
                var listItem = this.listThrumps.find(item => item.bid == wellItem.Id && item.username == username);
                if (listItem == null) {
                    var likeitem = { bid: wellItem.Id, username: username, upCount: 1, downCount: 0 };
                    this.listThrumps.push(likeitem);
                    this.WellUpCount = 0;
                    this.WellDownCount = 1;
                }
                else {
                    if (listItem.downCount > 0) {
                        this.$Message.error('You already unliked it !');
                    }
                    else {
                        listItem.upCount = 0;
                        listItem.downCount = 1;
                        this.WellUpCount = 0;
                        this.WellDownCount = 1;
                    }
                }
            },
            addImproveUp(improveItem) {
                let username = this.userName;
                var listItem = this.listThrumps.find(item => item.bid == improveItem.Id && item.username == username);
                if (listItem == null) {
                    var likeitem = { bid: improveItem.Id, username: username, upCount: 1, downCount: 0 };
                    this.listThrumps.push(likeitem);
                    this.ImproveUpCount = 1;
                    this.ImproveDownCount = 0;
                }
                else {
                    if (listItem.upCount > 0) {
                        this.$Message.error('You already liked it !');
                    }
                    else {
                        listItem.upCount = 1;
                        listItem.downCount = 0;
                        this.ImproveUpCount = 1;
                        this.ImproveDownCount = 0;
                    }
                }
            },
            addImproveDown(improveItem) {
                let username = this.userName;
                var listItem = this.listThrumps.find(item => item.bid == improveItem.Id && item.username == username);
                if (listItem == null) {
                    var likeitem = { bid: improveItem.Id, username: username, upCount: 0, downCount: 1 };
                    this.listThrumps.push(likeitem);
                    this.ImproveUpCount = 0;
                    this.ImproveDownCount = 1;
                }
                else {
                    if (listItem.downCount > 0) {
                        this.$Message.error('You already unliked it !');
                    }
                    else {
                        listItem.upCount = 0;
                        listItem.downCount = 1;
                        this.ImproveUpCount = 0;
                        this.ImproveDownCount = 1;
                    }
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
                    .withUrl("http://10.63.224.86:9070/BoardHub", {})
                    .configureLogging(signalR.LogLevel.Error)
                    .build();
                this.connection.on("ReceiveBoardItemMessage", boardid => {
                    if (boardid == this.boardId)
                    {
                        this.fetchData();
                    }
                });
                this.connection.start();
            },
            sendMsg() {
                this.connection.invoke("SendBoardItemMessage", this.boardId);
            },
            markCompleted() {
                this.$confirm(
                    {
                        message: 'Are you sure mark board [' + this.boardName +"] as Completed ? after that, you can't edit this board again",
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
                                    },
                                    headers: {
                                        'Authorization': 'Bearer ' + this.UserToken
                                    }
                                }).then(() => {
                                    this.renderFunc(this.boardName + ' is marked as completed successfully.');
                                }).then(() => {
                                    this.sendMsg();
                                })
                            }
                        }
                    }
                )
            },
            deleteBoard() {
                this.$confirm(
                    {
                        message: 'Are you sure delete board [' + this.boardName +'] ?',
                        button: {
                            no: 'No',
                            yes: 'Yes'
                        },
                        callback: confirm => {
                            if (confirm) {
                                this.axios.delete('/Board/' + this.boardId + '',
                                    {
                                        headers: {
                                            'Authorization': 'Bearer ' + this.UserToken
                                        }
                                    }).then(() => {
                                        this.$router.push('/boardAll');
                                    }).then(() => {
                                        this.renderFunc(this.boardName + ' is deleted successfully.');
                                    }).then(() => {
                                        this.sendMsg();
                                    })
                            }
                        }
                    }
                )
            }
        },
    }
</script>

<style>
    ul {
        list-style-type: none;
    }

    textarea {
        border-style: none !important;
    }

    .boardItemContent {
        margin-bottom: 15px;
    }

    .css-vubbuv {
        user-select: none;
        width: 1em;
        height: 1em;
        display: inline-block;
        fill: currentcolor;
        flex-shrink: 0;
        transition: fill 200ms cubic-bezier(0.4, 0, 0.2, 1) 0ms;
        font-size: 1.5rem;
    }

    .css-b7766g {
        display: inline-flex;
        -webkit-box-align: center;
        align-items: center;
        -webkit-box-pack: center;
        justify-content: center;
        position: relative;
        box-sizing: border-box;
        -webkit-tap-highlight-color: transparent;
        background-color: transparent;
        outline: 0px;
        border: 0px currentcolor;
        margin: 0px;
        cursor: pointer;
        user-select: none;
        vertical-align: middle;
        appearance: none;
        text-decoration: none;
        font-family: Roboto, Helvetica, Arial, sans-serif;
        font-weight: 500;
        font-size: 0.875rem;
        line-height: 1.75;
        letter-spacing: 0.02857em;
        text-transform: uppercase;
        min-width: 64px;
        border-radius: 4px;
        transition: background-color 250ms cubic-bezier(0.4, 0, 0.2, 1) 0ms, box-shadow 250ms cubic-bezier(0.4, 0, 0.2, 1) 0ms, border-color 250ms cubic-bezier(0.4, 0, 0.2, 1) 0ms, color 250ms cubic-bezier(0.4, 0, 0.2, 1) 0ms;
        color: inherit;
    }

    .top {
        padding: 10px;
        background: rgba(0, 153, 229, .7);
        color: #fff;
        text-align: center;
        border-radius: 5px;
    }

</style>