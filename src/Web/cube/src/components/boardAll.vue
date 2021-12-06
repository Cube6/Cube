<template>
    <div>
        <div style="height: 600px">
            <table>
                <thead>
                    <tr>
                        <th width="200px">Id</th>
                        <th width="200px">Name</th>
                        <th width="200px">DateCreated</th>
                        <th width="200px">ViewDetails</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="board in post" :key="board.Id">
                        <td>{{ board.Id }}</td>
                        <td>{{ board.Name }}</td>
                        <td>{{ board.DateCreated }}</td>
                        <td><Button type="primary" :boardId="board.Id" @click="ViewBoard(board.Id)">View</Button></td>
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
                this.$router.push({ name:'boardDetail', params: { boradId: val } });
            }
        },
    }
</script>

<style>
</style>