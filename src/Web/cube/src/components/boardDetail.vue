<template>
    <div>
        <h1 style="width:100%;text-align:center; font-size:larger">
            <span>

                {{boardName}}

                <span style="color:forestgreen" v-if="state == 2">Completed</span>

                <Dropdown v-if="state != 2" style="float: right;position: relative; font-size:12pt; ">
                    <Icon type="ios-more" size="28"></Icon>
                    <DropdownMenu slot="list">
                        <DropdownItem v-on:click.native="fetchData(true)"><Icon type="ios-refresh" size="28" />Refresh</DropdownItem>
                        <DropdownItem v-on:click.native="markCompleted()"><Icon type="ios-checkmark" size="28" />Mark as Completed</DropdownItem>
                        <DropdownItem v-on:click.native="deleteBoard()"><Icon type="ios-close" size="28" />Delete</DropdownItem>
                    </DropdownMenu>
                </Dropdown>
                <!--<Icon type="ios-refresh" size="28" style="float: right;position: relative;" v-on:click.native="fetchData(true)" title="Refresh"></Icon>-->
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
                        <Input v-model="boardDetail.ActionDetail" placeholder="Action" search enter-button="Add" @on-search="addAction" />
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

                                    <Input v-model="well.Detail" class="boardItemContent" type="textarea" style="border-style: none" :autosize="true" @on-blur="updateBoardItem(well)" @on-change="boardItemChanged" />
                                    <p style="height:22px;">
                                        <a href="#" @click.prevent="addWellUp(well)" :title="thumbsUpUserNames(well.ThumbsUp)">
                                            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 64px;" >
                                                <i :ref="'item'+well.Id" class="fa fa-thumbs-o-up fa-2x" style="color:green" aria-hidden="true"></i>
                                                &nbsp;<p >{{well.ThumbsUp.length}}</p>
                                            </button>
                                        </a>
                                        <a href="#" style="float:right;" @click.prevent="deleteBoardItem(well)" title="Delete" v-if="well.CreatedUser==userName">
                                            <span aria-label="Delete" class="">
                                                <button class="css-b7766g" tabindex="-1" type="button" aria-label="Delete" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 42px;">
                                                     <i class="fa fa-trash-o fa-2x" style="color: rgb(239, 83, 80)" aria-hidden="true"></i>
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

                                    <Input v-model="imporve.Detail" class="boardItemContent" type="textarea" :autosize="true" @on-blur="updateBoardItem(imporve)"  @on-change="boardItemChanged"/>
                                    <p style="height:22px;">
                                        <a href="#"  @click.prevent="addImproveUp(imporve)"  :title="thumbsUpUserNames(imporve.ThumbsUp)">
                                            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 64px;">
                                                <i :ref="'item'+imporve.Id" class="fa fa-thumbs-o-up fa-2x" style="color:green" aria-hidden="true"></i>
                                                &nbsp;<p>{{imporve.ThumbsUp.length}}</p>
                                            </button>
                                        </a>
                                        <a href="#" style="float:right" @click.prevent="deleteBoardItem(imporve)" title="Delete" v-if="imporve.CreatedUser==userName">
                                            <span aria-label="Delete" class="">
                                                <button class="css-b7766g" tabindex="-1" type="button" aria-label="Delete" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 42px;">
                                                     <i class="fa fa-trash-o fa-2x" style="color: rgb(239, 83, 80)" aria-hidden="true"></i>
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

                                    <Input v-model="action.Detail" class="boardItemContent" type="textarea" :autosize="true" @on-blur="updateBoardItem(action)" @on-change="boardItemChanged" />

                                    <p style="height:22px; ">
                                        <a href="#" @click.prevent="addActionUp(action)" :title="thumbsUpUserNames(action.ThumbsUp)">
                                            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 64px;">
                                            <i :ref="'item'+action.Id" class="fa fa-thumbs-o-up fa-2x" style="color:green" aria-hidden="true"></i>
                                                &nbsp;<p>{{action.ThumbsUp.length}}</p>
                                            </button>
                                        </a>
                                        <a href="#" @click.prevent="deleteBoardItem(action)" title="Delete" style="float:right" v-if="action.CreatedUser==userName">
                                            <Button type="text" class="css-b7766g" tabindex="-1" aria-label="Delete" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 42px;">
                                                 <i class="fa fa-trash-o fa-2x" style="color: rgb(239, 83, 80)" aria-hidden="true"></i>
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
    const WentWellType = 1;
    const NeedsImproveType = 2;
    const ActionType = 3;

    const AddOperation = 'add';
    const UpdateOperation = 'update';
    const DeleteOperation = 'delete';

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
                boardItemTextChanged: false,
                boardName:null,
                boardId: null,
                connection: "",
                state:0,
            };
        },
        created() {
            this.boardId = this.$route.params.boardId;
            this.boardName = this.$route.params.boardName;
            this.state = this.$route.params.state;
            console.log(this.state);
            this.fetchData(true);
            this.UserToken = localStorage.getItem('TOKEN');
            this.userName = localStorage.getItem('LOGINUSER').toUpperCase();

            this.init();
        },
        computed: {

        },
        methods: {
            fetchData(forceRefresh) {

                var msg;
                if (forceRefresh) {
                    msg = this.$Message.loading({
                        content: 'Loading...',
                        duration: 0
                    });
                }

                this.axios.get('/BoardItem/' + this.boardId + '')
                    .then(all => {
                        this.WellContent = all.data.filter(item => item.Type == 1);
                        this.ImporveContent = all.data.filter(item => item.Type == 2);
                        this.ActionContent = all.data.filter(item => item.Type == 3);

                        if (forceRefresh) {
                            setTimeout(msg);
                        }
                        return;

                    }).catch(error => {

                        if (forceRefresh) {
                            setTimeout(msg);
                        }
                        console.log(error);
                    })
            },

            addBoardDetail(boardDetail, type) {
                if (!boardDetail || !boardDetail.trim()) {
                    return;
                }

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
                }).then((res) => {

                    var listOfItems = this.getListOfItems(type);
                    listOfItems.push(res.data);

                    this.renderFunc(boardDetail + ' is created successfully.');

                    var context = {
                        Operation: AddOperation,
                        BoardItem: res.data
                    };
                    this.sendBoardItemMsg(context);

                    this.boardDetail.WellDetail = "";
                    this.boardDetail.ImporveDetail = "";
                    this.boardDetail.ActionDetail = "";
                })
            },

            addWentWell() {
                this.addBoardDetail(this.boardDetail.WellDetail, WentWellType);          
            },

            addImporved() {
                this.addBoardDetail(this.boardDetail.ImporveDetail, NeedsImproveType);      
            },

            addAction() {
                this.addBoardDetail(this.boardDetail.ActionDetail, ActionType);
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

                                this.axios.delete('/BoardItem/' + boardItem.Id + '',
                                    {
                                        headers: {
                                            'Authorization': 'Bearer ' + this.UserToken
                                        }
                                    }).then(() => {
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
                    listOfItems = this.ImporveContent;
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

            removeBoardItemById(arr, id) {
                var index = this.findIndexOfBoardItems(arr, id);
                if (index === -1) {
                    return false;
                }
                return !!arr.splice(index, 1);
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
                            createduser: this.userName,
                            boardid: this.boardId
                        },
                        headers: {
                            'Authorization': 'Bearer ' + this.UserToken
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

            addActionUp(actionItem) {
                this.thumbsUpAction(this.ActionContent, actionItem);
            },

            addWellUp(wellItem) {
                this.thumbsUpAction(this.WellContent, wellItem);
            },

            addImproveUp(improveItem) {
                this.thumbsUpAction(this.ImporveContent, improveItem);
            },

            thumbsUpAction(listOfItems, item) {
                let username = this.userName;
                var improveItemCache = listOfItems.find(c => c.Id == item.Id);
                var listThrumps = improveItemCache.ThumbsUp;
                var isAdd = true;

                var listItem = listThrumps.find(th => th.CreatedUser == username);
                if (listItem == null) {
                    var likeitem = { BoardItemId: item.Id, CreatedUser: username, Id: 0, DateCreated: null, DateModified: null, Type: 0 };
                    listThrumps.push(likeitem);
                }
                else {
                    var index = listThrumps.findIndex(item => {
                        if (item.bid == item.Id && item.username == username) {
                            return true;
                        }
                    })
                    listThrumps.splice(index, 1);
                    isAdd = false;
                }
                let itemId = 'item' + item.Id;
                if (isAdd) {
                    this.$refs[itemId][0].attributes.class.value = "fa fa-thumbs-up fa-2x";
                    this.addThumps(item.Id, 0);
                }
                else {
                    this.$refs[itemId][0].attributes.class.value = "fa fa-thumbs-o-up fa-2x";
                    this.deleteThumps(item.Id);
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
                    .withUrl("http://10.63.224.86:5050/BoardHub", {})
                    .configureLogging(signalR.LogLevel.Error)
                    .build();
                this.connection.on("ReceiveBoardItemMessage", boardItemEvent => {
                    if (boardItemEvent.BoardItem.BoardId == this.boardId) {
                        var listOfItems = this.getListOfItems(boardItemEvent.BoardItem.Type);

                        if (boardItemEvent.Operation == DeleteOperation) {
                            this.removeBoardItemById(listOfItems, boardItemEvent.BoardItem.Id);
                        }
                        else if (boardItemEvent.Operation == AddOperation) {
                            var index = this.findIndexOfBoardItems(listOfItems, boardItemEvent.BoardItem.Id);
                            if (index == -1) {
                                listOfItems.push(boardItemEvent.BoardItem);
                            }
                        }
                        else if (boardItemEvent.Operation == UpdateOperation) {
                            var indexOfEltToBeUpdated = this.findIndexOfBoardItems(listOfItems, boardItemEvent.BoardItem.Id);
                            if (indexOfEltToBeUpdated != -1) {
                                listOfItems.splice(indexOfEltToBeUpdated, 1, boardItemEvent.BoardItem)
                            }
                        }
                        else {
                            this.fetchData(false);
                        }
                    }
                });

                //this.connection.on("ReceiveCommentMessage", commentEvent => {
                //    if (commentEvent.BoardId == this.boardId) {

                //        var listOfItems = this.getListOfItems(commentEvent.Type);
                //        if (commentEvent.Operation == DeleteOperation) {
                            
                //        }
                //        else if (commentEvent.Operation == AddOperation) {

                //        }
                //    }
                //});

                //this.connection.on("ReceiveUserMessage", userEvent => {
                //    if (userEvent.BoardId == this.boardId) {
                //        if (userEvent.Operation == DeleteOperation) {

                //        }
                //        else if (userEvent.Operation == AddOperation) {

                //        }
                //    }
                //});

                this.connection.start();
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

            thumbsUpUserNames(thumbsUp) {
                var names = ''
                for (var i = 0; i < thumbsUp.length; i++) {
                    names = names + thumbsUp[i].CreatedUser +'\n';
                }
                return names;
            },

            addThumps(boardItemId,thumpType) {
                this.axios({
                    method: 'post',
                    url: '/Comment',
                    data: {
                        BoardItemId: boardItemId,
                        Type: thumpType,
                        CreatedUser: this.userName
                    },
                    headers: {
                        'Authorization': 'Bearer ' + this.UserToken
                    }
                }).then(() => {
                    //this.sendBoardItemMsg();
                })
            },
            deleteThumps(boardItemId) {
                this.axios({
                    method: 'delete',
                    url: '/Comment',
                    params: {
                        borderItemId: boardItemId,
                        username: this.userName
                    },
                    headers: {
                        'Authorization': 'Bearer ' + this.UserToken
                    }
                }).then(() => {
                    //this.sendBoardItemMsg();
                })
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
                                        this.sendBoardMsg();
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
    @import "../styles/css/font-awesome.css";

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