<template>
    <div>
        <h1 style="width:100%;text-align:center; font-size:larger">
            <span>

                {{boardName}}

                <span style="color:forestgreen" v-if="state == 2">
                    [
                        <img src="../assets/Icons/completed.jpg" title="Completed" style="width:15px; height:15px;" >
                        Completed
                    ]
                </span>

                <Dropdown style="float: right;position: relative; font-size:12pt; ">
                    <Icon type="ios-more" size="28"></Icon>
                    <DropdownMenu slot="list">
                        <DropdownItem v-on:click.native="fetchData(true)"><Icon type="ios-refresh" size="28" />Refresh</DropdownItem>
                        <DropdownItem v-if="state != 2"  v-on:click.native="markCompleted()"><Icon type="ios-checkmark" size="28" />Mark as Completed</DropdownItem>
                        <DropdownItem v-if="state != 2 && boardCreatedUser==userName"  v-on:click.native="deleteBoard()"><Icon type="ios-close" size="28" />Delete</DropdownItem>
                        <DropdownItem v-on:click.native="exportData()"><Icon type="ios-code-download" size="28" />Export</DropdownItem>
                    </DropdownMenu>
                </Dropdown>

                <!--Online Users-->
                <!-- <img v-for="user in participants" :key="user" :src="getUserAvatar(user)" :title="user" style="float: left; width: 20px; height: 20px; border-radius: 50%; " /> -->
            </span>
        </h1>
        <br />
        <table width="100%">
            <thead>
                <tr v-if="state == 2">
                    <th width="33%">
                        What went well ?
                    </th>
                    <th width="33%">
                        What could be improved ?
                    </th>
                    <th width="34%">
                        Action Items
                    </th>
                </tr>
                <tr v-if="state != 2">
                    <th width="33%">
                        <Input v-model="boardDetail.WellDetail" placeholder="What went well ?" search enter-button="Add" @on-search="addWentWell" />
                    </th>
                    <th width="33%">
                        <Input v-model="boardDetail.ImproveDetail" placeholder="What could be improved ?" search enter-button="Add" @on-search="addImproved" />
                    </th>
                    <th width="34%">
                        <Input v-model="boardDetail.ActionDetail" placeholder="Action Items" search enter-button="Add" @on-search="addAction" />
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
                                            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 64px;">
                                                <i :class="thumbsUpClass(well.ThumbsUp)" style="color:#4CAF50" aria-hidden="true"></i>
                                                &nbsp;<p>{{thumbsUpCount(well.ThumbsUp)}}</p>
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
                            <li v-for="improve in ImproveContent" :key="improve.Id">
                                <Card style="width: 100%; text-align: left;">
                                    <img :src="getUserAvatar(improve.CreatedUser)" :title="improve.CreatedUser" style="float: right; width: 20px; height: 20px; border-radius: 50%; " />

                                    <Input v-model="improve.Detail" class="boardItemContent" type="textarea" :autosize="true" @on-blur="updateBoardItem(improve)" @on-change="boardItemChanged" />
                                    <p style="height:22px;">
                                        <a href="#" @click.prevent="addImproveUp(improve)" :title="thumbsUpUserNames(improve.ThumbsUp)">
                                            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 64px;">
                                                <i :class="thumbsUpClass(improve.ThumbsUp)" style="color:#4CAF50" aria-hidden="true"></i>
                                                &nbsp;<p>{{thumbsUpCount(improve.ThumbsUp)}}</p>
                                            </button>
                                        </a>
                                        <a href="#" style="float:right" @click.prevent="deleteBoardItem(improve)" title="Delete" v-if="improve.CreatedUser==userName && state != 2">
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
                                                <i :class="thumbsUpClass(action.ThumbsUp)" style="color:#4CAF50" aria-hidden="true"></i>
                                                &nbsp;<p>{{thumbsUpCount(action.ThumbsUp)}}</p>
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
        <Table :columns="csvColumns" :data="csvData" ref="tableForExport" v-show="false"></Table>
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
                ImproveContent: null,
                boardDetail: {
                    WellDetail: "",
                    ImproveDetail: "",
                    ActionDetail: "",
                },
                boardItemTextChanged: false,
                boardName:null,
                boardId: null,
                connection: "",
                state: 0,
                csvData:[],
                csvColumns: [],
                participants:[]
            };
        },
        created() {
            this.boardId = this.$route.params.boardId;
            this.boardName = this.$route.params.boardName;
            this.boardCreatedUser = this.$route.params.createdUser.toUpperCase();
            this.state = this.$route.params.state;
            
            this.UserToken = localStorage.getItem('TOKEN');
            this.userName = localStorage.getItem('LOGINUSER').toUpperCase();
            
            this.fetchData(true);
            this.init();

            this.participants.push(this.userName);
        },
        destroyed(){
            if(this.connection!=null){

                var context = {
                    Operation: DeleteOperation,
                    UserName: this.userName,
                    BoardId: this.boardId
                };
                this.sendUserMsg(context);

                console.log("Hub for Board Detail with " + this.connection.connectionId + "is stopped");
                this.connection.stop();
            }
        },
        computed: {

        },
        methods: {
            fetchData(forceRefresh) {

                var msg;
                if (forceRefresh) {
                    msg = this.$Message.loading({
                        content: 'Loading Board Items...',
                        duration: 0
                    });
                }

                this.axios.get('/BoardItem/' + this.boardId + '')
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
                    this.boardDetail.ImproveDetail = "";
                    this.boardDetail.ActionDetail = "";
                })
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
                this.thumbsUpAction(ActionType, this.ActionContent, actionItem);
            },

            addWellUp(wellItem) {
                this.thumbsUpAction(WentWellType, this.WellContent, wellItem);
            },

            addImproveUp(improveItem) {
                this.thumbsUpAction(NeedsImproveType, this.ImproveContent, improveItem);
            },

            thumbsUpAction(type, listOfItems, item) {
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
                        if (item.CreatedUser == username) {
                            return true;
                        }
                    })
                    listThrumps.splice(index, 1);
                    isAdd = false;
                }

                if (isAdd) {
                    this.addThumps(type, item.Id, 0);
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

                this.connection.on("ReceiveCommentMessage", commentEvent => {
                    if (commentEvent.BoardId == this.boardId) {
                        var listOfItems = this.getListOfItems(commentEvent.Type);

                        if (commentEvent.Operation == AddOperation) {
                            var improveItemCache1 = listOfItems.find(c => c.Id == commentEvent.Comment.BoardItemId);
                            var listThrumps1 = improveItemCache1.ThumbsUp;

                            var listItem = listThrumps1.find(th => th.CreatedUser == commentEvent.Comment.CreatedUser);
                            if (listItem == null) {
                                listThrumps1.push(commentEvent.Comment);
                            }
                        }
                        else if (commentEvent.Operation == DeleteOperation) {
                            var improveItemCache2 = listOfItems.find(c => c.Id == commentEvent.Comment.BoardItemId);
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
                    }
                });

                this.connection.on("ReceiveUserMessage", userEvent => {
                    if (userEvent.BoardId == this.boardId) {
                        if(userEvent.UserName != this.userName)
                        {
                            if (userEvent.Operation == AddOperation) {
                                this.$Notice.info({
                                    render: h => {
                                            return h('span', [
                                                userEvent.UserName + ' is joined the board.'
                                            ])}
                                });

                                var index = this.participants.indexOf(userEvent.UserName);
                                if(index == -1)
                                {
                                    this.participants.push(userEvent.UserName);
                                }
                            }
                            else if(userEvent.Operation == DeleteOperation){
                                this.$Notice.info({
                                    render: h => {
                                            return h('span', [
                                                userEvent.UserName + ' is left the board.'
                                            ])}
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
                    var context = {
                        Operation: AddOperation,
                        UserName: this.userName,
                        BoardId: this.boardId
                    };
                    this.sendUserMsg(context);
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
                var names = ''
                for (var i = 0; i < thumbsUp.length; i++) {
                    names = names + thumbsUp[i].CreatedUser +'\n';
                }
                return names;
            },

            thumbsUpCount(thumbsUp) {

                var len = thumbsUp.length;
                if(len>0)
                {
                    return len;
                }
                
                return '';
            },

            thumbsUpClass(thumbsUp){
                var index = thumbsUp.find(item=> item.CreatedUser == this.userName);
                if(index!=null)
                {
                    return 'fa fa-thumbs-up fa-2x';
                }
                else
                {
                    return 'fa fa-thumbs-o-up fa-2x';
                }
            },

            addThumps(type,boardItemId,thumpType) {
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
                    var comment = {
                        CreatedUser: this.userName,
                        Type: thumpType,
                        BoardItemId: boardItemId
                    };
                    var context = {
                        Operation: AddOperation,
                        BoardId: this.boardId,
                        Type:type,
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
                    },
                    headers: {
                        'Authorization': 'Bearer ' + this.UserToken
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

                this.csvData.push({ "content": this.boardName, "createdUser":"", "vote": "" });
                this.csvData.push({ "content": "", "createdUser":"",  "vote": "" });
                this.csvData.push({ "content": "Went Well", "createdUser":"Submitter",  "vote": "Votes" });
                this.getSortedItems(this.WellContent).forEach((value,index) => {
                    this.csvData.push({ "content": index+1 +'.'+ value.content, "createdUser":value.createdUser, "vote": value.vote });
                });

                this.csvData.push({ "content": "", "createdUser":"","vote": "" });
                this.csvData.push({ "content": "To Improve", "createdUser":"Submitter", "vote": "Votes" });
                this.getSortedItems(this.ImproveContent).forEach((value,index) => {
                    this.csvData.push({ "content": index+1 +'.'+ value.content, "createdUser":value.createdUser, "vote": value.vote });
                });

                this.csvData.push({ "content": "", "createdUser":"", "vote": "" });
                this.csvData.push({ "content": "Action Items", "createdUser":"Submitter", "vote": "Votes" });
                this.getSortedItems(this.ActionContent).forEach((value,index) => {
                    this.csvData.push({ "content": index+1 +'.'+ value.content, "createdUser":value.createdUser, "vote": value.vote });
                });

                this.$refs.tableForExport.exportCsv({
                    filename: this.boardName,
                    columns: this.csvColumns,
                    data: this.csvData
                })
            },
            getSortedItems(content)
            {
                let sortedItems = [];
                content.forEach((value) => {
                    sortedItems.push({ 
                        "content": value.Detail.replace(',','-'), 
                        "createdUser": value.CreatedUser, 
                        "vote": value.ThumbsUp.length 
                        });
                });

                sortedItems.sort(function(a, b) {
                    return b.vote-a.vote
                });

                return sortedItems;
            }
        }
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