<template>
    <div class="layout">
        <Header style="height:40px; background-color:#0747A6;">
            <Menu mode="horizontal" theme="primary" style="height:55px; background-color:transparent;" active-name="1">
                <div class="layout-logo">
                    <img src="../assets/logo.jpg" style="width:30px; height:30px; border-radius:50%; " />
                    <span style="position: relative; top:-10px;">
                        Cube System
                    </span>
                      <span style="position:relative;top:-16px;font-size:8pt;color:#deebff">
                        v0.9
                    </span>
                </div>
                <div class="layout-nav">
                    <MenuItem name="menuUsers">
                        <Tooltip content="Not implemented yet" placement="bottom-start">
                            <Icon type="md-analytics"></Icon>
                            Activities
                        </Tooltip>
                    </MenuItem>

                    <MenuItem name="menuUsers" @click.native="goToUsers()" >
                            <Icon type="md-person"></Icon>
                            Users
                    </MenuItem>

                    <MenuItem name="menuSettings">
                        <Tooltip content="Not implemented yet" placement="bottom-start">
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
                            <img :src="getLoginUserAvatar()" style="width: 28px; height: 28px; border-radius: 50%; margin-top: 16px " :title="userName" />
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
        <Layout :style="{minHeight: '90vh'}">
            <Sider ref="side1" hide-trigger collapsible :collapsed-width="78" 
            :style="{background: '#fff', margin:'5px 0', minHeight: '120px',}" v-model="isCollapsed">
                
                <Menu active-name="1-1" theme="light" width="auto" style="height:100%" :class="menuitemClasses">
                    <MenuItem name="1-0" :title="isCollapsed? activeProject :''">             
                        <Dropdown v-if="!isCollapsed"  placement="bottom">
                            <Icon type="ios-paper" />
                            <span style="margin-left:5px;">{{ activeProject }}</span>
                            <Icon type="ios-arrow-down" />
                            <DropdownMenu slot="list" style="minWidth:150px;">            
                                <DropdownItem v-for="project in projects" :key="project"
                                                @click.native="setActiveProject(project)">
                                    {{ project.Name }}
                                </DropdownItem>
                            </DropdownMenu>
                        </Dropdown>
                        <Dropdown v-if="isCollapsed" trigger="click" placement="right-start">
                            <span style="padding: 4px 0 0 5px; width:30px;height:30px;background-color:#F1F2F3;border-radius: 50%;">
                                {{ generateProjectShortName(activeProject) }}
                            </span>
                            <DropdownMenu slot="list" style="minWidth:150px;">            
                                <DropdownItem v-for="project in projects" :key="project"
                                                @click.native="setActiveProject(project)">
                                    {{ project.Name }}
                                </DropdownItem>
                            </DropdownMenu>
                        </Dropdown>
                    </MenuItem>
                    <MenuItem name="1-1" @click.native="fetchData(0)" :title="isCollapsed?'All Boards':''">
                        <Icon type="md-book"></Icon>
                        <span>All Boards</span>
                    </MenuItem>
                    <MenuItem name="1-2" @click.native="fetchData(1)" :title="isCollapsed?'In Progress':''">
                        <Icon type="md-time"></Icon>
                        <span>In Progress</span>
                    </MenuItem>
                    <MenuItem name="1-3" @click.native="fetchData(2)" :title="isCollapsed?'Completed':''">
                        <Icon type="md-checkbox-outline"></Icon>
                        <span>Completed</span>
                    </MenuItem>
                    <MenuItem name="1-4" @click.native="fetchData(3)" :title="isCollapsed?'Recycle Bin':''">
                        <Icon type="md-trash"></Icon>
                        <span>Recycle Bin</span>
                    </MenuItem>
                </Menu>
                <Icon @click.native="collapsedSider" 
                        style="width:auto;cursor:pointer; position:absolute;margin:-30px 10px 0 0;z-index:1000;right:10px" 
                        :type="isCollapsed?'ios-arrow-forward':'ios-arrow-back'"
                        :title="isCollapsed?'Expand Sidebar':'Collapse Sidebar'"
                        size="24" />
            </Sider>
            <Layout :style="{padding: '0 1px 0px 12px'}">
                <Breadcrumb :style="{margin: '12px 0 0 0'}" v-if="this.isCollapsed">
                    <BreadcrumbItem  @click.native="fetchData(0)" v-if="this.isCollapsed">&nbsp;&nbsp; Board</BreadcrumbItem>
                    <BreadcrumbItem :to='navURL' v-if="navName != null && this.isCollapsed">{{navName}}</BreadcrumbItem>
                </Breadcrumb>
                <Content :style="{minHeight: '280px', margin:'5px 0'}">
                    <Card style="height:100%">
                        <router-view :key="$route.path"></router-view>
                    </Card>
                </Content>
            </Layout>
        </Layout>
        <Footer class="layout-footer-center">
            &copy; 2023 <a href="https://github.com/Cube6" target="_blank">Cube6</a>, All Rights Reserved
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
            <p :style="pStyle">About Cube (v0.9)</p>
            <div class="my-drawer-profile">
                <Row>
                    <Col span="24">
                    A Modern Retrospective System based on Microservices Architecture.<br/>
                    </Col>
                </Row>
                <img :src="getLogo()" style="width: 90px; height: 90px; border-radius: 50%; margin-top: 16px " />
                <Row>
                    <Col span="24">
                    &copy; 2023 <a href="https://github.com/Cube6" target="_blank">Cube6</a>, All Rights Reserved
                    </Col>
                </Row>
            </div>
            <Divider />
            <p :style="pStyle">History</p>
            <div class="my-drawer-profile">
                <ChangeHistory/>
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
