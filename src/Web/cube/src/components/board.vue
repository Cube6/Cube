<template>
    <div class="layout">
        <Header>
            <Menu mode="horizontal" theme="dark" active-name="1">
                <div class="layout-logo">
                    <img src="../assets/logo.jpg" style="width:35px; height:35px; border-radius:50%; " />
                    <span style="position: relative; top:-8px;">
                        Cube System
                    </span>
                </div>
                <div class="layout-nav">
                    <MenuItem name="menuUsers">
                        <Icon type="md-person"></Icon>
                        Users
                    </MenuItem>
                    <MenuItem name="menuSettings">
                        <Icon type="md-settings"></Icon>
                        Settings
                    </MenuItem>

                    <MenuItem name="menuAbout">
                        <Icon type="md-water"></Icon>
                        About
                    </MenuItem>
                </div>
                <div class="layout-loginUser">
                    <img :src="getLoginUserAvatar()" style="width:25px; height:25px; border-radius:50%; margin-right:5px; " />
                        <Dropdown>
                            <a href="javascript:void(0)" style="color:inherit">
                                {{userName}}
                                <Icon type="ios-arrow-down"></Icon>
                            </a>
                            <DropdownMenu slot="list">
                                <DropdownItem>Change Password</DropdownItem>
                                <DropdownItem>My Profile</DropdownItem>
                                <DropdownItem>Register New</DropdownItem>
                                <DropdownItem @click.native="Logout">Logout</DropdownItem>
                            </DropdownMenu>
                        </Dropdown>
                </div>
            </Menu>
        </Header>
        <Layout :style="{minHeight: '100vh'}">
            <Sider ref="side1" hide-trigger collapsible :collapsed-width="78" :style="{background: '#fff'}" v-model="isCollapsed">
                <Menu active-name="1-1" theme="light" width="auto" :class="menuitemClasses">
                    <MenuItem name="1-1" to="/boardAll">
                        <Icon type="md-book"></Icon>
                        <span>Board</span>
                    </MenuItem>
                    <MenuItem name="1-2" to="/boardAll">
                        <Icon type="md-time"></Icon>
                        <span>In Progress</span>
                    </MenuItem>
                    <MenuItem name="1-3" to="/boardAll">
                        <Icon type="ios-paper"></Icon>
                        <span>Completed</span>
                    </MenuItem>
                    <MenuItem name="1-4" to="/boardAll">
                        <Icon type="md-trash"></Icon>
                        <span>Recycle Bin</span>
                    </MenuItem>
                </Menu>
            </Sider>
            <Layout :style="{padding: '0 24px'}">
                <Breadcrumb :style="{margin: '24px 0'}">
                    <Icon @click.native="collapsedSider" :class="rotateIcon" :style="{width:'auto', float:'left'}" type="md-menu" size="24"></Icon>
                    <BreadcrumbItem>Home</BreadcrumbItem>
                    <BreadcrumbItem>Board</BreadcrumbItem>
                    <BreadcrumbItem>ViewAll</BreadcrumbItem>
                </Breadcrumb>
                <Content :style="{padding: '5px', minHeight: '280px', background: '#fff'}">
                    <div style="text-align:right;margin:10px;">
                        <Button type="primary" @click="AddBoard()">AddBoard</Button>
                    </div>
                    <Card>
                        <router-view></router-view>
                    </Card>
                </Content>
            </Layout>
        </Layout>
        <Footer class="layout-footer-center">
        Cube System 1.0 <br/>
        2021-2022 &copy; Cube Tech Group <br /> 
        <a href="https://github.com/Cube6">https://github.com/Cube6</a></Footer>
    </div>
</template>
<script>
    import board from "../scripts/board.js";
    export default board;
</script>

<style>
    @import "../styles/board.css";
</style>
