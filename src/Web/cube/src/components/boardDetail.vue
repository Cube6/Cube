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
                        <Button type="primary" @click="AddWentWell()">Send</Button>
                    </td>
                    <td>
                        <ul>
                            <li v-for="imporve in post" :key="imporve.Id">
                                {{imporve.Name}}
                            </li>
                        </ul>
                        <textarea v-model="boardDetail.ImporveDetail"></textarea>
                        <Button type="primary" @click="AddImporved()">Send</Button>
                    </td>
                    <td>
                        <ul>
                            <li v-for="action in post" :key="action.Id">
                                {{action.Name}}
                            </li>
                        </ul>
                        <textarea v-model="boardDetail.ActionDetail"></textarea>
                        <Button type="primary" @click="AddAction()">Send</Button>
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
                this.axios.get('/BoardItem/' + this.$route.params.boardId + '')
                    .then(r => {
                        console.log(r.data);
                        this.post = JSON.parse(r.data);
                    return;
                }).catch(error => {
                    console.log(error);
                })
            },
            AddBoardDetail(boardDetail) {
                this.axios({
                    method: 'post',
                    url: '/BoardItem',
                    data: {
                        boardid: this.$route.params.boardId,
                        detail: boardDetail,
                        type: 2,
                    },
                }).then(r => r.json())
                    .then(json => {
                        this.post = json;
                        return;
                    }).catch(error => {
                        console.log(error);
                    })
            },
            AddWentWell() {
                this.AddBoardDetail(this.boardDetail.WellDetail);
            },
            AddImporved() {
                this.AddBoardDetail(this.boardDetail.ImporveDetail);
            },
            AddAction() {
                this.AddBoardDetail(this.boardDetail.ActionDetail);
            },
           
        },
    }
</script>

<style>
    ul {
        list-style-type: none;
    }
</style>