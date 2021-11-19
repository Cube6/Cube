<template>
        <div style="height: 600px">
            <table>
                <thead>
                    <tr>
                        <th width="30%">What went well</th>
                        <th width="40%">What need to be imporved</th>
                        <th width="30%">Actions</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="board in post" :key="board.Id">
                        <td>{{ board.Id }}</td>
                        <td>{{ board.Name }}</td>
                        <td>{{ board.DateCreated }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
</template>

<script>
    //import axios from 'axios';
    export default {
        data() {
            return {
                post: null
            };
        },
        methods: {
            addDiscussionBoard(name) {
                alert(name);
            }
        },
    }
</script>

<style>
</style>