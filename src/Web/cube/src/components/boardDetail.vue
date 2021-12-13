<template>
    <div style="height: 600px">
        <table>
            <thead>
                <tr>
                    <th width="400px">What went well</th>
                    <th width="400px">What need to be imporved</th>
                    <th width="200px">Actions</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <ul>
                            <li v-for="well in post" :key="well.Id">
                                {{well.Name}}
                            </li>
                        </ul>
                        <textarea v-model="boardDetail.WellDetail"></textarea>
                        <Button type="primary" @click="AddWentWell('boardDetail')">Send</Button>
                    </td>
                    <td>
                        <ul>
                            <li v-for="imporve in post" :key="imporve.Id">
                                {{imporve.Name}}
                            </li>
                        </ul>
                        <textarea v-model="boardDetail.ImporveDetail"></textarea>
                        <Button type="primary" @click="AddImporved('boardDetail')">Send</Button>
                    </td>
                    <td>
                        <ul>
                            <li v-for="action in post" :key="action.Id">
                                {{action.Name}}
                            </li>
                        </ul>
                        <textarea v-model="boardDetail.ActionDetail"></textarea>
                        <Button type="primary" @click="AddAction('boardDetail')">Send</Button>
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
                post: null,
                boardDetail: {
                    WellDetail: "",
                    ImporveDetail: "",
                    ActionDetail: "",
                }
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();
        },
        methods: {
            fetchData() {
                console.log(this.$route.params.boardId);
                this.axios.get('/Board/BoardItem',{
                    params: { id: this.$route.params.boardId }
                }).then(r => r.json())
                    .then(json => {
                    this.post = json;
                    return;
                }).catch(error => {
                    console.log(error);
                })
            },
            AddWentWell(boardDetail) {
                console.log(this.$refs[boardDetail]);
                console.log(this.$route.params.boardId);
                this.axios({
                    method: 'post',
                    url: '/Board/CreateBoardItem',
                    data: {
                        BoardId: this.$route.params.boardId,
                        detail: this.boardDetail.WellDetail,
                        type: 0,
                    },
                }).then(this.fetchData()).catch(error => {
                        console.log(error);
                    })
            },
            AddImporved(boardDetail) {
                console.log(this.$refs[boardDetail]);
                this.axios({
                    method: 'post',
                    url: '/Board/CreateBoardItem',
                    data: {
                        boardid: this.$route.params.boardId,
                        detail: this.boardDetail.ImporveDetail,
                        type: 1,
                    },
                }).then(r => r.json())
                    .then(json => {
                        this.post = json;
                        return;
                    }).catch(error => {
                        console.log(error);
                    })
            },
            AddAction(boardDetail) {
                console.log(this.$refs[boardDetail]);
                this.axios({
                    method: 'post',
                    url: '/Board/CreateBoardItem',
                    data: {
                        boardid: this.$route.params.boardId,
                        detail: this.boardDetail.ActionDetail,
                        type: 2,
                    },
                }).then(r => r.json())
                    .then(json => {
                        this.post = json;
                        return;
                    }).catch(error => {
                        console.log(error);
                    })
            }
        },
    }
</script>

<style>
    ul {
        list-style-type: none;
    }
</style>