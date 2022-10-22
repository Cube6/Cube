<template>
    <div style="margin:0">
        <Table border :columns="columns" :data="post"></Table>
        <!-- <table>
            <thead>
                <tr>
                    <th width="40%">Name</th>
                    <th width="20%">Items Submitted(Count)</th>
                    <th width="25%">Action</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="user in post" :key="user.Id">
                    <td>
                        <img :src="getUserAvatar(user.CreatedUser)" :title="'Owner: ' + user.CreatedUser" style="width:20px; height:20px; border-radius:50%; " />
                        {{ user.CreatedUser }}
                    </td>
                    <td>{{ user.Count }}</td>
                    <td>
                        <Button type="primary" :boardId="user.Id" >View</Button>
                        &nbsp;
                        <Button type="error" :boardId="user.Id" >Delete</Button>
                    </td>
                </tr>
            </tbody>
        </table> -->
    </div>
</template>

<script>
    import users from "../scripts/users.js";
    export default users;
</script>

<style>
.table {
    font: Helvetica;
    font-size:12pt;
    line-height:40px;
    width: 100%;
    height: 100%;
}
</style>