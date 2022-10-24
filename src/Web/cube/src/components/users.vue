<template>
    <div class="usersTable">
        <Table border :columns="columns" :data="data" :loading="loading">
            <template #name="{ row }">
            <img :src="getUserAvatar(row.CreatedUser)" style="width:20px; height:20px; border-radius:50%; " />
            &nbsp;
            {{ row.CreatedUser }}
        </template>

        <template #action="{ row, index }">
            <Button type="primary" size="small" style="margin-right: 5px" @click="show(index)">View</Button>
            <!-- <Button type="error" size="small" @click="remove(index)">Delete</Button> -->
        </template>
        
        </Table>
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
.usersTable{
    minHeight: 300px;
	height: 100%;
	width: 100%;
	padding: 12px;
	position: absolute;
	overflow-y: auto;
}
.table {
    font: Helvetica;
    font-size:12pt;
    line-height:40px;
    width: 100%;
    height: 100%;
}
</style>