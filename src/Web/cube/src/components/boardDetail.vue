<template>
    <div>
        <table width="100%">
            <thead>
                <tr>
                    <th width="33%">
                        <Input v-model="boardDetail.WellDetail" placeholder="What went well ?" search enter-button="Send" @on-search="addWentWell" />
                    </th>
                    <th width="33%">
                        <Input v-model="boardDetail.ImporveDetail" placeholder="What could be improved ?" search enter-button="Send" @on-search="addImporved" />
                    </th>
                    <th width="34%">
                        <Input v-model="boardDetail.ActionDetail" placeholder="A brilliant idea to share ?" search enter-button="Send" @on-search="addAction" />
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td style="vertical-align:top">
                        <ul>
                            <li v-for="well in WellContent" :key="well.Id">
                                <Card style="width: 100%; text-align: left;">
                                    <Input v-model="well.Detail" type="textarea" :autosize="true" @on-blur="updateBoardItem(well)" />
                                    <p slot="title" style="height:22px;">
                                        <span aria-label="" class="">
                                            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 64px;">
                                                <svg class="css-vubbuv" focusable="false" viewBox="0 0 24 24" aria-hidden="true" style="color: rgb(46, 125, 50);">
                                                    <use xlink:href="#at-handUp"></use>
                                                </svg>
                                                &nbsp;0
                                            </button>
                                        </span>
                                        <span aria-label="" class="">
                                            <button class="MuiButton-root MuiButton-text MuiButton-textInherit MuiButton-sizeMedium MuiButton-textSizeMedium MuiButton-colorInherit MuiButtonBase-root Mui-disabled css-b7766g" tabindex="-1" type="button" disabled="" aria-label="Dislike" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 64px;">
                                                <svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeMedium css-vubbuv" focusable="false" viewBox="0 0 24 24" aria-hidden="true" data-testid="ThumbDownOutlinedIcon" style="color: rgb(239, 83, 80);">
                                                    <use xlink:href="#at-handDown"></use>
                                                </svg>
                                                &nbsp;0
                                            </button>
                                        </span>
                                    </p>
                                    <a href="#" slot="extra" @click.prevent="deleteBoardItem(well)" title="Delete">
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
                    </td>
                    <td style="vertical-align:top">
                        <ul>
                            <li v-for="imporve in ImporveContent" :key="imporve.Id">
                                <Card style="width: 100%; text-align: left;" >
                                    <Input v-model="imporve.Detail" type="textarea" :autosize="true" @on-blur="updateBoardItem(imporve)" />
                                    <p slot="title" style="height: 22px;">
                                        <span aria-label="" class="">
                                            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 64px;">
                                                <svg class="css-vubbuv" focusable="false" viewBox="0 0 24 24" aria-hidden="true" style="color: rgb(46, 125, 50);">
                                                    <use xlink:href="#at-handUp"></use>
                                                </svg>
                                                &nbsp;0
                                            </button>
                                        </span>
                                        <span aria-label="" class="">
                                            <button class="MuiButton-root MuiButton-text MuiButton-textInherit MuiButton-sizeMedium MuiButton-textSizeMedium MuiButton-colorInherit MuiButtonBase-root Mui-disabled css-b7766g" tabindex="-1" type="button" disabled="" aria-label="Dislike" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 64px;">
                                                <svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeMedium css-vubbuv" focusable="false" viewBox="0 0 24 24" aria-hidden="true" data-testid="ThumbDownOutlinedIcon" style="color: rgb(239, 83, 80);">
                                                    <use xlink:href="#at-handDown"></use>
                                                </svg>
                                                &nbsp;0
                                            </button>
                                        </span>
                                    </p>
                                    <a href="#" slot="extra" @click.prevent="deleteBoardItem(imporve)" title="Delete">
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
                    </td>
                    <td style="vertical-align:top">
                        <ul>
                            <li v-for="action in ActionContent" :key="action.Id">
                                <Card style="width: 100%; text-align: left; ">
                                    <Input v-model="action.Detail" type="textarea" :autosize="true" @on-blur="updateBoardItem(action)" />
                                    <p slot="title" style="height: 22px;">
                                        <span aria-label="" class="">
                                            <button :id="action.Id" @on-click="addActionUp(action)" class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 10px; padding-right: 10px; min-width: 64px;">
                                                <svg class="css-vubbuv" focusable="false" viewBox="0 0 24 24" aria-hidden="true" style="color: rgb(46, 125, 50);">
                                                    <use xlink:href="#at-handUp"></use>
                                                </svg>
                                                &nbsp;0
                                            </button>
                                        </span>
                                        <span aria-label="" class="">
                                            <Button @on-click="addActionDown(action)" class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 10px; padding-top:0px; padding-right: 10px; min-width: 64px;">
                                                <svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeMedium css-vubbuv" focusable="false" viewBox="0 0 24 24" aria-hidden="true" data-testid="ThumbDownOutlinedIcon" style="color: rgb(239, 83, 80);">
                                                    <use xlink:href="#at-handDown"></use>
                                                </svg>
                                                &nbsp;0
                                            </Button>
                                        </span>
                                    </p>
                                    <a href="#" slot="extra" @click.prevent="deleteBoardItem(action)" title="Delete">
                                        <span aria-label="Delete" class="">
                                            <Button type="text" class="css-b7766g" tabindex="-1" aria-label="Delete" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 42px;">
                                                <svg class="css-vubbuv" focusable="false" viewBox="0 0 24 24" aria-hidden="true" style="color: rgb(239, 83, 80);">
                                                    <use xlink:href="#at-delete"></use>
                                                </svg>
                                            </Button>
                                        </span>
                                    </a>
                                </Card>
                            </li>
                        </ul>
                    </td>
                </tr>
            </tbody>
        </table>
   </div>
</template>

<script>

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
                ActionUpCount: null,
                ActionDownCount: null,
                WellUpCount: null,
                WellDownCount: null,
                ImporveUpCount: null,
                ImporveDownCount: null,
                boardDetail: {
                    WellDetail: "",
                    ImporveDetail: "",
                    ActionDetail: "",
                },
                listThrumps: [
                    { userid: null,bid: null, upCount:0,downCount:0 }
                ]
            };
        },
        created() {
            this.fetchData();
            this.UserToken = localStorage.getItem('TOKEN');
            this.userName = localStorage.getItem('LOGINUSER').toUpperCase();
        },
        methods: {
            fetchData() {
                this.axios.get('/BoardItem/' + this.$route.params.boardId + '')
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
                        boardid: this.$route.params.boardId,
                        detail: boardDetail,
                        type: type,
                        createduser: this.userName
                    },
                    headers: {
                        'Authorization': 'Bearer ' + this.UserToken
                    }
                }).then(() => {
                    this.fetchData();
                })
            },
            addWentWell() {
                this.addBoardDetail(this.boardDetail.WellDetail,1);
            },
            addImporved() {
                this.addBoardDetail(this.boardDetail.ImporveDetail,2);
            },
            addAction() {
                this.addBoardDetail(this.boardDetail.ActionDetail,3);
            },
            deleteBoardItem(boardItem) {
                this.axios.delete('/BoardItem/' + boardItem.Id + '',
                    {
                        headers: {
                            'Authorization': 'Bearer ' + this.UserToken
                    }})
                    .then(() => {
                        this.fetchData();
                    })
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
                        boardid: this.$route.params.boardId
                    },
                    headers: {
                        'Authorization': 'Bearer ' + this.UserToken
                    }
                }).then(() => {
                    this.fetchData();
                })
            },
            addActionUp(actionItem) {
                let username = this.userName;
                this.listThrumps.some((item) => {
                    if (item.bid == actionItem.id && item.username == username) {
                        if (item.upCount > 0) {
                            this.$Message.error('You already liked it !');
                        }
                        else {
                            item.upCount = 1;
                            item.downCount = 0;
                        }
                    }
                    else {
                        var likeitem = { bid: actionItem.id, username: username, upCount: 1, downCount: 0 };
                        this.listThrumps.push(likeitem);
                    }

                });
            },
            addActionDown(actionItem) {
                let username = this.userName;
                this.listThrumps.some((item) => {
                    if (item.bid == actionItem.id && item.username == username) {
                        if (item.downCount > 0) {
                            this.$Message.error('You already unliked it !');
                        }
                        else {
                            item.upCount = 0;
                            item.downCount = 1;
                        }
                    }
                    else {
                        var likeitem = { bid: actionItem.id, username: username, upCount: 0, downCount: 1 };
                        this.listThrumps.push(likeitem);
                    }
                });
            },
            addWellUp() {

            },
            addWellDown() {

            },
            addImproveUp() {

            },
            addImproveDown() {

            }
        },
    }
</script>

<style>
    ul {
        list-style-type: none;
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

</style>