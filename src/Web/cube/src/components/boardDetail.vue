<template>
    <div>
        <table>
            <thead>
                <tr>
                    <th width="400px">
                        <Input v-model="boardDetail.WellDetail" placeholder="What went well ?" search enter-button="Send" @on-search="addWentWell" />
                    </th>
                    <th width="400px">
                        <Input v-model="boardDetail.ImporveDetail" placeholder="What could be improved ?" search enter-button="Send" @on-search="addImporved" />
                    </th>
                    <th width="400px">
                        <Input v-model="boardDetail.ActionDetail" placeholder="A brilliant idea to share ?" search enter-button="Send" @on-search="addAction" />
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td style="vertical-align:top">
                        <ul>
                            <li v-for="well in WellContent" :key="well.Id">
                                <Card style="width:380px">
                                    <Input v-model="well.Detail" type="textarea" :autosize="true" @on-blur="updateBoardItem(well)" />
                                    <p slot="title">
                                        <Icon type="ios-film-outline"></Icon>
                                    </p>
                                    <a href="#" slot="extra" @click.prevent="deleteBoardItem(well)">
                                        <Icon type="ios-loop-strong"></Icon>
                                        Delete
                                    </a>
                                </Card>
                            </li>
                        </ul>
                    </td>
                    <td style="vertical-align:top">
                        <ul>
                            <li v-for="imporve in ImporveContent" :key="imporve.Id">
                                <Card style="width:380px">
                                    <Input v-model="imporve.Detail" type="textarea" :autosize="true" @on-blur="updateBoardItem(imporve)" />
                                    <p slot="title">
                                        <Icon type="ios-film-outline"></Icon>
                                    </p>
                                    <a href="#" slot="extra" @click.prevent="deleteBoardItem(imporve)">
                                        <Icon type="ios-loop-strong"></Icon>
                                        Delete
                                    </a>
                                </Card>
                            </li>
                        </ul>
                    </td>
                    <td style="vertical-align:top">
                        <ul>
                            <li v-for="action in ActionContent" :key="action.Id">
                                <Card style="width:380px">
                                    <Input v-model="action.Detail" type="textarea" :autosize="true" @on-blur="updateBoardItem(action)"/>
                                    <p slot="title">
                                        <Icon type="ios-film-outline"></Icon>
                                    </p>
                                    <a href="#" slot="extra" @click.prevent="deleteBoardItem(action)">
                                        <Icon type="ios-loop-strong"></Icon>
                                        Delete
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
                ActionContent: null,
                WellContent: null,
                ImporveContent: null,
                deleteActionId: null,
                deleteWellId: null,
                deleteImporveId: null,
                boardDetail: {
                    WellDetail: "",
                    ImporveDetail: "",
                    ActionDetail: "",
                }
            };
        },
        created() {
            this.fetchData();
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
            addBoardDetail(boardDetail,type) {
                this.axios({
                    method: 'post',
                    url: '/BoardItem',
                    data: {
                        boardid: this.$route.params.boardId,
                        detail: boardDetail,
                        type: type,
                    },
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
                this.axios.delete('/BoardItem/' + boardItem.Id + '')
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
                        boardid: this.$route.params.boardId
                    },
                }).then(() => {
                    this.fetchData();
                })
            },
        },
    }
</script>

<style>
    ul {
        list-style-type: none;
    }
</style>