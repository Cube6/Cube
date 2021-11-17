<template>
    <div class="layout">
        <Layout :style="{minHeight: '100vh'}">
            <Sider collapsible :collapsed-width="78" v-model="isCollapsed">
                <Menu active-name="1-1" theme="dark" width="auto" :class="menuitemClasses">
                    <MenuItem name="1-1">
                        <Icon type="ios-navigate"></Icon>
                        <span>Discussboard</span>
                    </MenuItem>
                    <MenuItem name="1-2">
                        <Icon type="search"></Icon>
                        <span>User</span>
                    </MenuItem>
                    <MenuItem name="1-3">
                        <Icon type="settings"></Icon>
                        <span>Settings</span>
                    </MenuItem>
                </Menu>
            </Sider>
            <Layout>
                <Header :style="{background: '#fff', boxShadow: '0 2px 3px 2px rgba(0,0,0,.1)'}"></Header>
                <Content :style="{padding: '0 16px 16px'}">
                    <Breadcrumb :style="{margin: '16px 0'}">
                        <BreadcrumbItem>Home</BreadcrumbItem>
                        <BreadcrumbItem>Components</BreadcrumbItem>
                        <BreadcrumbItem>Board</BreadcrumbItem>
                    </Breadcrumb>
                    <Card>
                        <div style="height: 600px">
                            <table>
                                <thead>
                                    <tr>
                                        <th width="200px">Id</th>
                                        <th width="200px">Name</th>
                                        <th width="200px">DateCreated</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="user in post" :key="user.id">
                                        <td>{{ user.id }}</td>
                                        <td>{{ user.name }}</td>
                                        <td>{{ user.datecreated }}</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </Card>
                </Content>
            </Layout>
        </Layout>
    </div>
</template>
<script>
    import board from "../scripts/board.js";
    export default board;
</script>

<style>
    @import "../styles/board.css";
</style>
