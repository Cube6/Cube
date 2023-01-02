<template>
<fullscreen v-model="fullscreen" :teleport="teleport" :page-only="pageOnly">
    <div class="subLayout">
        <h1 style="width:100%;text-align:center; font-size:15pt;">
            <span>
                <span :ref="'editBoardName'" @keydown="updateBoardNameKeydown($event)" @blur="updateBoardName()" :contenteditable="state == 1">{{boardName}}</span>

                <span style="color:forestgreen" v-if="state == 2">
                    <img src="../assets/Icons/completed.jpg" title="Completed" style="width:15px; height:15px;" >
                    Completed
                </span>

                <Dropdown v-if="state != 2" v-on:click.native="toggleOnlineUsers()">
                    <!-- <Icon type="md-people" size="24"></Icon> -->
                    <div style="cursor: pointer;padding-left:10px;color:#00ad00" >
                        <i class="fa fa-users" aria-hidden="true"></i>
                        <span>{{participants.length}}</span>
                    </div>
                    <DropdownMenu slot="list">
                        <div style="margin:10px;">
                            <img v-for="user in participants" :key="user" :src="getUserAvatar(user)" :title="user" style="width: 30px; height: 30px; border-radius: 50%; " />
                        </div>
                    </DropdownMenu>
                </Dropdown>

                <Dropdown style="float: right;position: relative; font-size:12pt; ">
                    <Icon type="ios-more" size="28"></Icon>
                    <DropdownMenu slot="list">
                        <!-- <DropdownItem v-on:click.native="fetchData(true)"><Icon type="ios-refresh" size="28" />Refresh</DropdownItem> -->
                        <DropdownItem v-if="state != 2"  v-on:click.native="markCompleted()"><Icon type="ios-checkmark" size="28" />Mark as Completed</DropdownItem>
                        <DropdownItem v-if="state != 2 && boardCreatedUser.toUpperCase()==userName"  v-on:click.native="deleteBoard()"><Icon type="ios-close" size="28" />Delete</DropdownItem>
                        <DropdownItem v-on:click.native="exportData()"><Icon type="ios-code-download" size="28" />Export</DropdownItem>
                    </DropdownMenu>
                </Dropdown>

                <!-- Full Screen -->
                <Icon v-if="!fullscreen" type="md-qr-scanner" size="18" style="float: right;cursor: pointer;margin-top:8px;margin-right:5px;" 
                        v-on:click.native="toggle()" title="Full Screen">
                </Icon>

                <a v-if="fullscreen" href="#" @click.prevent="toggle()" title="Exit Full Screen">
                    <button class="css-b7766g" tabindex="-1" style="float: right;cursor: pointer;margin-top:10px;margin-right:5px;" >
                        <i class="fa fa-window-restore fa-1x" style="color:#FFAA66" aria-hidden="true"></i>
                    </button>
                </a>

                <!-- Refresh -->
                <Icon type="ios-refresh" size="24" style="float: right;cursor: pointer;margin-top:6px;margin-right:5px;" 
                        v-on:click.native="fetchData(true)" title="Refresh">
                </Icon>

                <a href="#" @click.prevent="sortItems()" :title="sortButtonTitle">
                    <button class="css-b7766g" tabindex="-1" style="float: right; margin-top:10px;margin-right:10px;">
                        <i :class="sortButtonClass" style="color:#666666" aria-hidden="true"></i>
                    </button>
                </a>

                <!--Online Users-->    
                <img v-for="user in participants" :hidden='showParticipants==false'  :key="user" :src="getUserAvatar(user)" :title="user" style="float: left; width: 24px; height: 24px; border-radius: 50%; " />
            </span>
        </h1>
        <br />
        <table width="100%">
            <tr style="vertical-align:top">
                <td width="33.2%">
                    <table width="100%">
                        <thead>
                            <tr v-if="state == 2">
                                <th width="100%">
                                    Went well / Liked / keep doing (<span style="color:#50AA62; font-weight: bolder;font-style: italic;">{{ WellContent.length }}</span> )
                                </th>
                            </tr>
                            <tr v-if="state != 2">
                                <th width="100%">
                                    <Input element-id="wentWell" v-model="boardDetail.WellDetail" class="wellInputContent" type="textarea" :autosize="{maxRows: 4}" placeholder="Went well / Liked / Keep doing" spellcheck :loading="loading" @keydown.native="handleKeydown" />
                                </th>
                            </tr>
                        </thead>
                    </table>          
                </td>
                <td width="33.2%">
                    <table width="100%">
                        <thead>
                            <tr v-if="state == 2">
                                <th width="100%">
                                    Can be improved / Lacked / Stop doing (<span style="color:#E8897A; font-weight: bolder;font-style: italic;">{{ ImproveContent.length }}</span> )
                                </th>
                            </tr>
                            <tr v-if="state != 2">
                                <th width="100%">
                                    <Input element-id="improved" v-model="boardDetail.ImproveDetail" class="improveInputContent" type="textarea" :autosize="{maxRows: 4}" placeholder="Can be improved / Lacked / Stop doing" spellcheck @keydown.native="handleKeydown" />
                                </th>
                            </tr>
                        </thead>
                    </table>            
                </td>
                <td width="33.2%">
                    <table width="100%">
                        <thead>
                            <tr v-if="state == 2">
                                <th width="100%">
                                    An action / An idea / A suggestion / A change (<span style="color:#7A93E8; font-weight: bolder;font-style: italic;">{{ ActionContent.length }}</span> )
                                </th>
                            </tr>
                            <tr v-if="state != 2">
                                <th width="100%">
                                    <Input element-id="action" v-model="boardDetail.ActionDetail" class="actionInputContent" type="textarea" :autosize="{maxRows: 4}" placeholder="An action /An idea / A suggestion/ A change" spellcheck @keydown.native="handleKeydown" />
                                </th>
                            </tr>
                        </thead>
                    </table>
                </td>
            </tr>       
        </table>
        <table width="100%" height="100%" style="margin-top:-5px;" @click="cleanFocusedImproveItem(currentFocusImproveItemId)">
            <tr style="vertical-align:top">
                <td width="33.2%" style="background-color: #F4F5F7;padding: 4px; border-radius: 0.5ch;">
                    <table width="100%">
                        <tbody>
                            <tr>
                                <td style="vertical-align:top;">
                                    <ul>
                                        <li v-for="well in WellContent" :key="well.Id">
                                            <Card style="width: 100%; text-align: left; margin:0px 0px 3px 0px;" class="boardItemCard"> 
                                                <Tooltip :content="well.CreatedUser" placement="bottom">
                                                    <!-- background: #F1F3F1 -->
                                                    <img :src="getUserAvatar(well.CreatedUser)" style="width: 24px; height: 24px; border-radius: 50%; margin-bottom: 5px;" />
                                                </Tooltip>
                                                <!-- Menu Items -->
                                                <Dropdown style="float: right;position: relative; font-size:12pt; ">
                                                    <Icon type="ios-more" size="28"></Icon>
                                                    <DropdownMenu slot="list">
                                                    <DropdownItem :disabled="!canDeleteBoardItem(well)"  v-on:click.native="canDeleteBoardItem(well)?deleteBoardItem(well):''"><i class="fa fa-trash-o fa-2x" style="color: rgb(239, 83, 80)" aria-hidden="true"></i><span style="color:red">&nbsp;Delete</span></DropdownItem>
                                                    </DropdownMenu>
                                                </Dropdown>

                                                <Input v-model="well.Detail" class="boardItemContent wellItem" type="textarea" :readonly="!canEditBoardItem(well)" spellcheck style="border-style: none" :autosize="true" @on-blur="updateBoardItem(well)" @on-change="boardItemChanged" />
                                                
                                                <CommentList :boardItem="well" :boardItemType="1" :listOfItems="WellContent"/>
                                            </Card>
                                        </li>
                                    </ul>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
                <td width="2px"></td>
               <td width="33.2%" style="background-color: #F4F5F7;padding: 4px;border-radius: 0.5ch;">
                     <table width="100%">
                        <tbody>
                            <tr>
                                <td style="vertical-align:top">
                                    <ul>
                                        <div v-if="hasNoItemsInTheBoard()"
                                            class="noItemsStyle">
                                            Speak with the soul and achieve your goal. Speak
                                            <img src="../assets/Icons/share.png" title="Your Voice Matters" style="width:100px;height:50px;opacity:30%;" >
                                        </div>
                                        <li v-for="improve in ImproveContent" :key="improve.Id">
                                            <Card ref="improvedItemCard" :id="improve.Id" :style="{'opacity': setImproveItemOpacity(improve.Id)}" style="width: 100%; text-align: left; margin:0px 0px 3px 0px;"  class="boardItemCard" @click.native.stop="refocusImproveItem(improve.Id)">
                                                <Tooltip :content="improve.CreatedUser" placement="bottom">
                                                    <!-- background: #FBF5F5 -->                                               
                                                    <img :src="getUserAvatar(improve.CreatedUser)" style="float: left;width: 24px; height: 24px; border-radius: 50%; margin-bottom: 5px; margin-left: 5px;" />
                                                </Tooltip>

                                                <!-- Menu Items -->
                                                <Dropdown style="float: right;position: relative; font-size:12pt; ">
                                                    <Icon type="ios-more" size="28"></Icon>
                                                    <DropdownMenu slot="list">
                                                        <DropdownItem v-on:click.native="addAssociatedActionItem(improve.Id)"><i class="fa fa-plus fa-2x" aria-hidden="true"></i>&nbsp;Add Action Item</DropdownItem>
                                                        <DropdownItem v-on:click.native="markBoardItem(improve, 2)"  v-if="improve.State != 2"><i class="fa fa-check-circle fa-2x" style="color: #29984F" aria-hidden="true"></i>&nbsp;Mark as Done</DropdownItem>
                                                        <DropdownItem v-on:click.native="markBoardItem(improve, 1)"  v-if="improve.State != 1"><i class="fa fa-clock-o fa-2x" style="color: #5AC967" aria-hidden="true"></i>&nbsp;Mark as In Progress</DropdownItem>
                                                        <DropdownItem :disabled="!canDeleteBoardItem(improve)"  v-on:click.native="canDeleteBoardItem(improve)?deleteBoardItem(improve):''"><i class="fa fa-trash-o fa-2x" style="color: rgb(239, 83, 80)" aria-hidden="true"></i><span style="color:red">&nbsp;Delete</span></DropdownItem>
                                                        <!-- <DropdownItem v-on:click.native="exportData()"><i class="fa fa-hand-o-right fa-2x" style="color: rgb(80, 83, 239)" aria-hidden="true"></i>&nbsp;Take Action</DropdownItem> -->
                                                        </DropdownMenu>
                                                </Dropdown>
                                                <span v-if="improve.State == 2" style="float: right; margin-right: 10px; color: #006644">
                                                    <i class="fa fa-check-circle fa-1x" style="" aria-hidden="true"></i>
                                                    &nbsp;
                                                    <span style="background-color: #E3FCEF; padding: 1px 3px; border-radius: 5px;">
                                                        <b>DONE</b>
                                                    </span>
                                                </span>

                                                <Input v-model="improve.Detail" :class="getBoardItemClass(2, improve.State)" type="textarea" :readonly="!canEditBoardItem(improve)" spellcheck :autosize="true" @on-blur="updateBoardItem(improve)" @on-change="boardItemChanged" />
                                                
                                                <CommentList :boardItem="improve" :boardItemType="2" :listOfItems="ImproveContent"/>
                                            </Card>
                                        </li>
                                    </ul>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
                <td width="2px"></td>
                <td width="33.2%" style="background-color: #F4F5F7;padding:4px;border-radius: 0.5ch;">  
                    <table width="100%">
                        <tbody>
                            <tr>
                                <td style="vertical-align:top">
                                    <ul>
                                        <div ref="actionItemBlock" :style="{'height': actionItemBlockOffset + 'px'}"></div>
                                        <li v-for="action in ActionContent" :key="action.Id">
                                            <transition name="transition-drop">
                                                <Card v-show="currentFocusImproveItemId == null || currentFocusImproveItemId == action.AssociatedBoardItemId" style="width: 100%; text-align: left; margin:0px 0px 3px 0px;" class="boardItemCard" @click.native.stop="focusAssociatedImproveItem(action.AssociatedBoardItemId)">
                                                    <Poptip :ref="generateAssigneeList(action.Id)" @on-popper-show="showAssignees(action.Id)" @on-popper-hide="clearAssignees">
                                                        <template #content>
                                                            <Input prefix="ios-contact" placeholder="Choose an assignee" v-model="assigneeKeyWord" @on-change="filterAssignees" clearable  />
                                                            <div style="min-width:300px; height: 300px; margin-top: 10px; overflow-y: scroll; ">
                                                                <p v-for="assignee in assigneesFilter" :key="assignee" @click.prevent="selectAssignee(action, assignee)" 
                                                                class="assigneeItemStyle"
                                                                style="cursor:pointer;padding-top: 10px; padding-bottom: 10px; padding-left: 10px;">
                                                                    <img :src="getUserAvatar(assignee)" 
                                                                    style="float: left; cursor:pointer; width: 24px; height: 24px; border-radius: 50%; margin-bottom: 5px; margin-right: 10px;" />
                                                                    {{ assignee }}
                                                                </p>
                                                            </div>
                                                        </template>
                                                        <Tooltip :content="'Assignee:' + action.Assignee" placement="bottom">
                                                            <!-- background: #ECF5FC -->
                                                            <img :src="getUserAvatar(action.Assignee)" style="float: left; cursor:pointer; width: 24px; height: 24px; border-radius: 50%; margin-bottom: 5px;" />
                                                        </Tooltip>
                                                    </Poptip>
                                                    <!-- Menu Items -->
                                                     <Dropdown style="float: right;position: relative; font-size:12pt; ">
                                                        <Icon type="ios-more" size="28"></Icon>
                                                        <DropdownMenu slot="list">
                                                            <DropdownItem v-on:click.native="markBoardItem(action, 2)"  v-if="action.State != 2"><i class="fa fa-check-circle fa-2x" style="color: #29984F" aria-hidden="true"></i>&nbsp;Mark as Done</DropdownItem>
                                                            <DropdownItem v-on:click.native="markBoardItem(action, 1)"  v-if="action.State != 1"><i class="fa fa-clock-o fa-2x" style="color: #5AC967" aria-hidden="true"></i>&nbsp;Mark as In Progress</DropdownItem>
                                                            <DropdownItem :disabled="!canDeleteBoardItem(action)"  v-on:click.native="canDeleteBoardItem(action)?deleteBoardItem(action):''"><i class="fa fa-trash-o fa-2x" style="color: rgb(239, 83, 80)" aria-hidden="true"></i><span style="color:red">&nbsp;Delete</span></DropdownItem>
                                                        </DropdownMenu>
                                                    </Dropdown>
                                                    
                                                    <span v-if="action.State == 2" style="float: right; margin-left: 10px; color: #006644;">
                                                        <i class="fa fa-check-circle fa-1x" style="" aria-hidden="true"></i>
                                                        &nbsp;
                                                        <span style="background-color: #E3FCEF; padding: 1px 3px; border-radius: 5px;">
                                                            <b>DONE</b>
                                                        </span>
                                                    </span>

                                                    <Input v-model="action.Detail" :class="getBoardItemClass(3, action.State)" type="textarea" :readonly="!canEditBoardItem(action)" spellcheck :autosize="true" @on-blur="updateBoardItem(action)" @on-change="boardItemChanged" />

                                                    <CommentList :boardItem="action" :boardItemType="3" :listOfItems="ActionContent"/>
                                                </Card>
                                            </transition>
                                        </li>
                                    </ul>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
        <Table :columns="csvColumns" :data="csvData" ref="tableForExport" v-show="false"></Table>
        <BackTop :height="100" :bottom="120" :right="50">
            <div class="top">Back to Top</div>
        </BackTop>
    </div>
</fullscreen>
</template>

<script>
	import boardDetail from "../scripts/boardDetail.js";
	export default boardDetail;
</script>

<style>
  @import "../styles/css/font-awesome.css";
	@import "../styles/boardDetail.css";
</style>