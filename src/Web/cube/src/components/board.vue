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
                    <img src="../assets/Person/Michael.jpg" style="width:25px; height:25px; border-radius:50%; margin-right:5px; " />
                        <Dropdown>
                            <a href="javascript:void(0)" style="color:inherit">
                                CubeAdmin
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
                    <Card style="height:100%">
                        <router-view></router-view>
                    </Card>
                </Content>
            </Layout>
        </Layout>
        <Footer class="layout-footer-center">
            Cube System 1.0 <br />
            2021-2022 &copy; Cube Tech Group <br />
            <a href="https://github.com/Cube6">https://github.com/Cube6</a>
        </Footer>

        <symbol id="at-plus" viewBox="0 0 1024 1024"><path d="M476.16 476.16V191.0272c0-20.6848 16.0256-37.4272 35.84-37.4272 19.8144 0 35.84 16.7424 35.84 37.4272V476.16h285.1328c20.6848 0 37.4272 16.0256 37.4272 35.84 0 19.8144-16.7424 35.84-37.4272 35.84H547.84v285.1328c0 20.6848-16.0256 37.4272-35.84 37.4272-19.8144 0-35.84-16.7424-35.84-37.4272V547.84H191.0272C170.3424 547.84 153.6 531.8144 153.6 512c0-19.8144 16.7424-35.84 37.4272-35.84H476.16z"></path></symbol>
        <symbol id="at-handUp">
            <path d="M9 21h9c.83 0 1.54-.5 1.84-1.22l3.02-7.05c.09-.23.14-.47.14-.73v-2c0-1.1-.9-2-2-2h-6.31l.95-4.57.03-.32c0-.41-.17-.79-.44-1.06L14.17 1 7.58 7.59C7.22 7.95 7 8.45 7 9v10c0 1.1.9 2 2 2zM9 9l4.34-4.34L12 10h9v2l-3 7H9V9zM1 9h4v12H1z"></path>
        </symbol>
        <symbol id="at-handDown" >
            <path d="M15 3H6c-.83 0-1.54.5-1.84 1.22l-3.02 7.05c-.09.23-.14.47-.14.73v2c0 1.1.9 2 2 2h6.31l-.95 4.57-.03.32c0 .41.17.79.44 1.06L9.83 23l6.59-6.59c.36-.36.58-.86.58-1.41V5c0-1.1-.9-2-2-2zm0 12-4.34 4.34L12 14H3v-2l3-7h9v10zm4-12h4v12h-4z"></path>
        </symbol>
        <symbol id="at-delete">
            <path d="M14.12 10.47 12 12.59l-2.13-2.12-1.41 1.41L10.59 14l-2.12 2.12 1.41 1.41L12 15.41l2.12 2.12 1.41-1.41L13.41 14l2.12-2.12zM15.5 4l-1-1h-5l-1 1H5v2h14V4zM6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM8 9h8v10H8V9z"></path>
        </symbol>
    </div>
</template>
<script>
    import board from "../scripts/board.js";
    export default board;
</script>

<style>
    @import "../styles/board.css";
</style>
