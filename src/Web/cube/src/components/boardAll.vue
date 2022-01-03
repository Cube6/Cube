<template>
        <div style="margin:0">
            <table style="width:100%;height:100%;">
                <thead>
                    <tr>
                        <th width="5%">Id</th>
                        <th width="40%">Name</th>
                        <th width="20%">Date Created</th>
                        <th width="10%">Created User</th>
                        <th width="25%">Action</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="board in post" :key="board.Id">
                        <td>{{ board.Id }}</td>
                        <td>{{ board.Name }}</td>
                        <td>{{ board.DateCreated }}</td>
                        <td>
                        <img src="../assets/Person/Michael.jpg" style="width:20px; height:20px; border-radius:50%; "/>
                        {{ board.CreatedUser }}</td>
                        <td>
                            <Button type="primary" :boardId="board.Id" @click="ViewBoard(board.Id)">View</Button>
                            &nbsp;
                            <Button type="error" :boardId="board.Id" @click="DeleteBoard(board.Id)">Delete</Button>
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