<template>
    <div class="board">
        <div class="content">
            <table>
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Temp. (C)</th>
                        <th>Temp. (F)</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="user in post" :key="user.id">
                        <td>{{ user.id }}</td>
                        <td>{{ user.name }}</td>
                        <td>{{ user.avatarUrl }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script lang="js">
    import board from "../scripts/board.js";
    export default board;
</script>