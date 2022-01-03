<template>
    <div class="layout">
        <Header>
            <Menu mode="horizontal" theme="dark" active-name="1">
                <div class="layout-logo"></div>
                <div class="layout-nav">
                    <MenuItem name="menuUsers">
                        <Icon type="md-person"></Icon>
                        Users
                    </MenuItem>
                    <MenuItem name="menuSettings">
                        <Icon type="md-settings"></Icon>
                        Settings
                    </MenuItem>
                </div>
                <div class="layout-loginUser">
                    <Icon type="ios-paper"></Icon>
                    Admin
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
                <Content :style="{padding: '24px', minHeight: '280px', background: '#fff'}">
                    <!--<Header :style="{background: '#fff', boxShadow: '0 2px 3px 2px rgba(0,0,0,.1)'}">
                        <div>
                            <Button type="primary" @click="AddBoard()">AddBoard</Button>
                        </div>
                    </Header>-->

                    <Header :style="{background: '#fff'}">
                        <div>
                            <Button type="primary" @click="AddBoard()">AddBoard</Button>
                        </div>
                    </Header>
                    <Card>
                        <router-view></router-view>
                    </Card>
                </Content>
            </Layout>
        </Layout>
        <Footer class="layout-footer-center">
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
