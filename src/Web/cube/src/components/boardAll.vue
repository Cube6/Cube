<template>
    <div>
        <div style="margin:0">
            <table>
                <thead>
                    <tr>
                        <th width="100px">Id</th>
                        <th width="500px">Name</th>
                        <th width="200px">Date Created</th>
                        <th width="400px">Action</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="board in post" :key="board.Id">
                        <td>{{ board.Id }}</td>
                        <td>{{ board.Name }}</td>
                        <td>{{ board.DateCreated }}</td>
                        <td>
                            <Button type="primary" :boardId="board.Id" @click="ViewBoard(board.Id)">View</Button>
                            &nbsp;
                            <Button type="error" :boardId="board.Id" @click="DeleteBoard(board.Id)">Delete</Button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                post: null
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();
        },
        methods: {
            fetchData() {
                fetch('Board')
                    .then(r => r.json())
                    .then(json => {
                        this.post = json;
                        return;
                    });
            },
            ViewBoard(val) {
                this.$router.push({ name:'boardDetail', params: { boardId: val } });
            }
        },
    }
</script>

<style>
</style>