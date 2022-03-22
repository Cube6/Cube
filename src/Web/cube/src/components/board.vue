<template>
    <div class="layout">
        <Header style="height:70px;">
            <Menu mode="horizontal" theme="dark" active-name="1">
                <div class="layout-logo">
                    <img src="../assets/logo.jpg" style="width:35px; height:35px; border-radius:50%; " />
                    <span style="position: relative; top:-8px;">
                        Cube
                    </span>
                </div>
                <div class="layout-nav">
                    <MenuItem name="menuUsers">
                        <Tooltip content="Not implemented yet" placement="bottom-start" theme="light">
                            <Icon type="md-person"></Icon>
                            Users
                        </Tooltip>
                    </MenuItem>

                    <MenuItem name="menuSettings">
                        <Tooltip content="Not implemented yet" placement="bottom-start" theme="light">
                            <Icon type="md-settings"></Icon>
                            Settings
                        </Tooltip>
                    </MenuItem>

                    <MenuItem name="menuAbout"  @click.native="showAboutView = true">
                        <Icon type="md-water"></Icon>
                        About
                    </MenuItem>

                    <MenuItem name="menuProfile" style="float:right">
                        <Dropdown>
                            <img :src="getLoginUserAvatar()" style="width: 25px; height: 25px; border-radius: 50%; margin-top: 16px " :title="userName" />
                            <DropdownMenu slot="list">            
                                <DropdownItem>Change Password</DropdownItem>
                                <DropdownItem @click.native="showMyProfile = true">My Profile</DropdownItem>
                                <DropdownItem @click.native="createAccount">Register New</DropdownItem>
                                <DropdownItem><hr style="border:1px solid #CCC" /></DropdownItem>
                                <DropdownItem @click.native="logout">Log Out</DropdownItem>
                            </DropdownMenu>
                        </Dropdown>
                    </MenuItem>
                </div>
            </Menu>
        </Header>
        <Layout :style="{minHeight: '100vh'}">
            <Sider ref="side1" hide-trigger collapsible :collapsed-width="78" :style="{background: '#fff'}" v-model="isCollapsed">
                <Menu active-name="1-1" theme="light" width="auto" :class="menuitemClasses">
                    <MenuItem name="1-1" @click.native="fetchData(null)">
                        <Icon type="md-book"></Icon>
                        <span>Board</span>
                    </MenuItem>
                    <MenuItem name="1-2" @click.native="fetchData(1)">
                        <Icon type="md-time"></Icon>
                        <span>In Progress</span>
                    </MenuItem>
                    <MenuItem name="1-3" @click.native="fetchData(2)">
                        <Icon type="ios-paper"></Icon>
                        <span>Completed</span>
                    </MenuItem>
                    <MenuItem name="1-4" @click.native="fetchData(3)">
                        <Icon type="md-trash"></Icon>
                        <span>Recycle Bin</span>
                    </MenuItem>
                </Menu>
            </Sider>
            <Layout :style="{padding: '0 12px'}">
                <Breadcrumb :style="{margin: '12px 0'}">
                    <Icon @click.native="collapsedSider" :class="rotateIcon" :style="{width:'auto', float:'left'}" type="md-menu" size="24"></Icon>
                    <BreadcrumbItem  @click.native="fetchData(null)" v-if="this.isCollapsed">&nbsp;&nbsp; Board</BreadcrumbItem>
                    <BreadcrumbItem :to='navURL' v-if="navName != null && this.isCollapsed">{{navName}}</BreadcrumbItem>
                </Breadcrumb>
                <Content :style="{padding: '5px', minHeight: '280px', background: '#fff'}">
                    <Card style="height:100%">
                        <router-view :key="$route.path"></router-view>
                    </Card>
                </Content>
            </Layout>
        </Layout>
        <Footer class="layout-footer-center">
            &copy; 2022 <a href="https://github.com/Cube6">Cube6</a>, All Rights Reserved
        </Footer>

        <symbol id="at-plus" viewBox="0 0 1024 1024"><path d="M476.16 476.16V191.0272c0-20.6848 16.0256-37.4272 35.84-37.4272 19.8144 0 35.84 16.7424 35.84 37.4272V476.16h285.1328c20.6848 0 37.4272 16.0256 37.4272 35.84 0 19.8144-16.7424 35.84-37.4272 35.84H547.84v285.1328c0 20.6848-16.0256 37.4272-35.84 37.4272-19.8144 0-35.84-16.7424-35.84-37.4272V547.84H191.0272C170.3424 547.84 153.6 531.8144 153.6 512c0-19.8144 16.7424-35.84 37.4272-35.84H476.16z"></path></symbol>
        
        <Drawer :closable="false" width="640" v-model="showMyProfile">
            <p :style="pStyle">My Profile</p>
            <p :style="pStyle">Personal</p>
            <div class="my-drawer-profile">
                <img :src="getLoginUserAvatar()" style="width: 80px; height: 80px; border-radius: 50%; margin-top: 16px " />
                <Row>
                    <Col span="12">
                    Name: {{ userName }}
                    </Col>
                    <Col span="12">
                    Dept: ECS-Data Exchange
                    </Col>
                </Row>
                <Row>
                    <Col span="12">
                    City: Shanghai
                    </Col>
                    <Col span="12">
                    Country: China
                    </Col>
                </Row>
            </div>
            <Divider />
            <p :style="pStyle">Statistics</p>
            <div class="my-drawer-profile">
                <Row>
                    <Col span="12">
                    Board: created X boards
                    </Col>
                    <Col span="12">
                    Items: submitted X items
                    </Col>
                </Row>
            </div>
            <Divider />
            <!--<p :style="pStyle">Contacts</p>
            <div class="my-drawer-profile">
                <Row>
                    <Col span="12">
                    Email: admin@aresn.com
                    </Col>
                    <Col span="12">
                    Phone Number: +86 18888888888
                    </Col>
                </Row>
                <Row>
                    <Col span="12">
                    GitHub: <a href="https://github.com/view-design/ViewUI" target="_blank">https://github.com/view-design/ViewUI</a>
                    </Col>
                </Row>
            </div>-->
            <div class="my-drawer-footer">
                <Button type="primary" @click="showMyProfile = false">Close</Button>
            </div>
        </Drawer>
        <Drawer :closable="false" width="640" v-model="showAboutView">
            <p :style="pStyle">About Cube</p>
            <p :style="pStyle">Cube v0.2</p>
            <div class="my-drawer-profile">
                <img :src="getLogo()" style="width: 80px; height: 80px; border-radius: 50%; margin-top: 16px " />
                <Row>
                    <Col span="24">
                    &copy; 2022 <a href="https://github.com/Cube6">Cube6</a>, All Rights Reserved
                    </Col>
                </Row>
            </div>
            <Divider />
            <p :style="pStyle">History</p>
            <div class="my-drawer-profile">
                <Row>
                    <Col span="4">
                    2022-03-18
                    </Col>
                    <Col span="4">
                    Version 0.3
                    </Col>
                    <Col span="16">
                    -Support Spellcheck, Sort Items by Votes
                    </Col>
                </Row>
                <Row>
                    <Col span="4">
                    2022-03-13
                    </Col>
                    <Col span="4">
                    Version 0.2
                    </Col>
                    <Col span="16">
                    -Export to CSV, Filter Boards by status,Fix bugs
                    </Col>
                </Row>
                <Row>
                    <Col span="4">
                    2022-03-01
                    </Col>
                    <Col span="4">
                    Version 0.1
                    </Col>
                    <Col span="16">
                    -Release the first version of Cube.
                    </Col>
                </Row>
            </div>
            <Divider />
            <div class="my-drawer-footer">
                <Button type="primary" @click="showAboutView = false">Close</Button>
            </div>
        </Drawer>
    </div>
</template>
<script>
    import board from "../scripts/board.js";
    export default board;
</script>

<style>
    @import "../styles/board.css";
</style>
