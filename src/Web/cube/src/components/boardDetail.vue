<template>
<fullscreen v-model="fullscreen" :teleport="teleport"  :page-only="pageOnly">
    <div class="subLayout">
        <h1 style="width:100%;text-align:center; font-size:larger">
            <span>
                <span :ref="'editBoardName'" @keydown="updateBoardNameKeydown($event)" @blur="updateBoardName()" :contenteditable="state == 1">{{boardName}}</span>

                <span style="color:forestgreen" v-if="state == 2">
                        <img src="../assets/Icons/completed.jpg" title="Completed" style="width:15px; height:15px;" >
                        Completed
                </span>

                <Dropdown  v-if="state != 2" v-on:click.native="toggleOnlineUsers()">
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
                <img v-for="user in participants" :hidden='showParticipants==false'  :key="user" :src="getUserAvatar(user)" :title="user" style="float: left; width: 20px; height: 20px; border-radius: 50%; " />
            </span>
        </h1>
        <br />
        <table width="100%">
            <thead>
                <tr v-if="state == 2">
                    <th width="33%">
                        What went well ?
                    </th>
                    <th width="33%">
                        What could be improved ?
                    </th>
                    <th width="34%">
                        Action Items
                    </th>
                </tr>
                <tr v-if="state != 2">
                    <th width="33%">
                        <Input v-model="boardDetail.WellDetail" class="wellInputContent" placeholder="What went well ?" spellcheck :loading="loading" search enter-button="Add" @on-search="addWentWell" />
                    </th>
                    <th width="33%">
                        <Input v-model="boardDetail.ImproveDetail" class="improveInputContent" placeholder="What could be improved ?" spellcheck search enter-button="Add" @on-search="addImproved" />
                    </th>
                    <th width="34%">
                        <Input v-model="boardDetail.ActionDetail" class="actionInputContent" placeholder="Action Items" spellcheck search enter-button="Add" @on-search="addAction" />
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td style="vertical-align:top">
                        <ul>
                            <li v-for="well in WellContent" :key="well.Id">
                                <Card style="width: 100%; text-align: left; margin:0px 0px 3px 0px;"> 
                                     <!-- background: #F1F3F1 -->
                                    <img :src="getUserAvatar(well.CreatedUser)" :title="well.CreatedUser" style="float: right; width: 20px; height: 20px; border-radius: 50%; " />

                                    <Input v-model="well.Detail" class="boardItemContent wellItem" type="textarea" :readonly="!canEditBoardItem(well)" spellcheck style="border-style: none" :autosize="true" @on-blur="updateBoardItem(well)" @on-change="boardItemChanged" />
                                    <p style="height:22px;">
                                        <a href="#" @click.prevent="addWellUp(well)" :title="thumbsUpUserNames(well.ThumbsUp)">
                                            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 64px;">
                                                <i :class="thumbsUpClass(well.ThumbsUp)" aria-hidden="true"></i>
                                                &nbsp;<p>{{thumbsUpCount(well.ThumbsUp)}}</p>
                                            </button>
                                        </a>

                                        <!-- Comments -->
                                        <a v-if="!well.ToggleComment && well.Messages.length==0" href="#" @click.prevent="toggleComment(well)" title="Reply">
                                            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 32px;">
                                                <i :class="replyClass(well.ThumbsUp)" aria-hidden="true"></i>
                                            </button>
                                        </a>
                                         <a v-if="!well.ToggleComment && well.Messages.length > 0" href="#" @click.prevent="toggleComment(well)" :title="thumbsUpUserNames(well.ThumbsUp)">
                                            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 32px;font-size: 8pt;">
                                                <p>{{thumbsUpCount(well.Messages)}} Replies&nbsp;^</p>
                                            </button>
                                        </a>
                                        <a v-if="well.ToggleComment" href="#" @click.prevent="toggleComment(well)" :title="thumbsUpUserNames(well.ThumbsUp)">
                                            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 32px;font-size: 8pt;">
                                                <p>Collapse All&nbsp;^</p>
                                            </button>
                                        </a>

                                        <!-- Menu Items -->
                                        <Dropdown style="float: right;position: relative; font-size:12pt; ">
                                            <Icon type="ios-more" size="28"></Icon>
                                            <DropdownMenu slot="list">
                                              <DropdownItem :disabled="!canDeleteBoardItem(well)"  v-on:click.native="canDeleteBoardItem(well)?deleteBoardItem(well):''"><i class="fa fa-trash-o fa-2x" style="color: rgb(239, 83, 80)" aria-hidden="true"></i>&nbsp;Delete</DropdownItem>
                                            </DropdownMenu>
                                        </Dropdown>
                                    </p>

                                    <!-- Comments -->
                                    <ul v-if="well.ToggleComment" >
                                        <li>
                                            <table style="width:100%">
                                                  <thead>
                                                    <tr>
                                                        <th width="8%">
                                                        </th>
                                                        <th width="20px">
                                                        </th>
                                                        <th width="92%">
                                                        </th>
                                                        <th width="10px">
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tr v-for="message in well.Messages" :key="message.Id">
                                                    <td></td>
                                                    <td style="vertical-align:top;"> 
                                                        <img :src="getUserAvatar(message.CreatedUser)" :title="message.CreatedUser" style="width: 20px; height: 20px; border-radius: 50%; " />
                                                    </td>
                                                    <td style="padding-bottom:8px"> 
                                                        <!-- <span class="commentUserName">
                                                            {{message.CreatedUser}}
                                                        </span> -->
                                                        <Input v-model="message.Detail" :class="commentContentClass(message)" type="textarea" :readonly="!canEditComment(message)" :autosize="true" placeholder="Reply..."  @on-blur="updateWellCommentItem(message)" @on-change="commentItemChanged" />
                                                        <br/>            
                                                    </td>
                                                    <td style="vertical-align:top;">
                                                        <a href="#" style="float:right" v-if="canDeleteComment(message)" @click.prevent="deleteWellComment(message)" title="Delete">
                                                            <span aria-label="Delete" class="">
                                                                <button class="css-b7766g" tabindex="-1" type="button" aria-label="Delete">
                                                                     <i class="fa fa-times fa-1x deleteComment" aria-hidden="true"></i>
                                                                </button>
                                                            </span>
                                                        </a>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td></td>
                                                    <td> 
                                                        <img :src="getUserAvatar(userName)" :title="userName" style="width: 20px; height: 20px; border-radius: 50%; " />
                                                    </td>
                                                    <td> 
                                                        <Input v-model="well.Comment.Detail" class="commentInputContent" placeholder="Reply..." spellcheck :loading="loading" @on-enter="addWellComment(well)" />
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </li>
                                    </ul>
                                    
                                </Card>
                            </li>
                        </ul>
                    </td>
                    <td style="vertical-align:top">
                        <ul>
                            <div v-if="hasNoItemsInTheBoard()"
                                class="noItemsStyle">
                                Speak with the soul and achieve your goal. Speak
                                <img src="../assets/Icons/share.png" title="Your Voice Matters" style="width:100px;height:50px;opacity:30%;" >
                            </div>
                            <li v-for="improve in ImproveContent" :key="improve.Id">
                                <Card style="width: 100%; text-align: left; margin:0px 0px 3px 0px;">
                                     <!-- background: #FBF5F5 -->
                                    <span v-if="improve.State == 2" style="float: left; margin-left:5px; color: #CCCCD0">
                                        <i class="fa fa-check-circle fa-1x" style="" aria-hidden="true"></i>
                                        &nbsp;<b>DONE</b>
                                    </span>
                        
                                    <img :src="getUserAvatar(improve.CreatedUser)" :title="improve.CreatedUser" style="float: right; width: 20px; height: 20px; border-radius: 50%; " />
                                    <Input v-model="improve.Detail" :class="getBoardItemClass(2, improve.State)" type="textarea" :readonly="!canEditBoardItem(improve)" spellcheck :autosize="true" @on-blur="updateBoardItem(improve)" @on-change="boardItemChanged" />
                                    <p style="height:22px;">
                                        <a href="#" @click.prevent="addImproveUp(improve)" :title="thumbsUpUserNames(improve.ThumbsUp)">
                                            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 64px;">
                                                <i :class="thumbsUpClass(improve.ThumbsUp)" aria-hidden="true"></i>
                                                &nbsp;<p>{{thumbsUpCount(improve.ThumbsUp)}}</p>
                                            </button>
                                        </a>

                                        <!-- <a href="#" style="float:right" @click.prevent="deleteBoardItem(improve)" title="Delete" v-if="improve.CreatedUser==userName && state != 2">
                                            <span aria-label="Delete" class="">
                                                <button class="css-b7766g" tabindex="-1" type="button" aria-label="Delete" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 42px;">
                                                    <i class="fa fa-trash-o fa-2x" style="color: rgb(239, 83, 80)" aria-hidden="true"></i>
                                                </button>
                                            </span>
                                        </a> -->

                                        <!-- Comments -->
                                        <a v-if="!improve.ToggleComment && improve.Messages.length==0" href="#" @click.prevent="toggleComment(improve)" title="Reply">
                                            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 32px;">
                                                <i :class="replyClass(improve.ThumbsUp)" aria-hidden="true"></i>
                                            </button>
                                        </a>
                                         <a v-if="!improve.ToggleComment && improve.Messages.length > 0" href="#" @click.prevent="toggleComment(improve)" :title="thumbsUpUserNames(improve.ThumbsUp)">
                                            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 32px;font-size: 8pt;">
                                                <p>{{thumbsUpCount(improve.Messages)}} Replies&nbsp;^</p>
                                            </button>
                                        </a>
                                        <a v-if="improve.ToggleComment" href="#" @click.prevent="toggleComment(improve)" :title="thumbsUpUserNames(improve.ThumbsUp)">
                                            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 32px;font-size: 8pt;">
                                                <p>Collapse All&nbsp;^</p>
                                            </button>
                                        </a>

                                        <!-- Menu Items -->
                                        <Dropdown style="float: right;position: relative; font-size:12pt; ">
                                            <Icon type="ios-more" size="28"></Icon>
                                            <DropdownMenu slot="list">
                                              <DropdownItem :disabled="!canDeleteBoardItem(improve)"  v-on:click.native="canDeleteBoardItem(improve)?deleteBoardItem(improve):''"><i class="fa fa-trash-o fa-2x" style="color: rgb(239, 83, 80)" aria-hidden="true"></i>&nbsp;Delete</DropdownItem>
                                              <DropdownItem v-on:click.native="markBoardItem(improve, 2)"  v-if="improve.State != 2"><i class="fa fa-check-circle fa-2x" style="color: #29984F" aria-hidden="true"></i>&nbsp;Mark as Done</DropdownItem>
                                              <DropdownItem v-on:click.native="markBoardItem(improve, 1)"  v-if="improve.State != 1"><i class="fa fa-clock-o fa-2x" style="color: #5AC967" aria-hidden="true"></i>&nbsp;Mark as In Progress</DropdownItem>
                                                <!-- <DropdownItem v-on:click.native="exportData()"><i class="fa fa-hand-o-right fa-2x" style="color: rgb(80, 83, 239)" aria-hidden="true"></i>&nbsp;Take Action</DropdownItem> -->
                                            </DropdownMenu>
                                        </Dropdown>
                                    </p>

                                    <!-- Comments -->
                                    <ul v-if="improve.ToggleComment" >
                                        <li>
                                            <table style="width:100%">
                                                  <thead>
                                                    <tr>
                                                        <th width="8%">
                                                        </th>
                                                        <th width="20px">
                                                        </th>
                                                        <th width="92%">
                                                        </th>
                                                        <th width="10px">
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tr v-for="message in improve.Messages" :key="message.Id">
                                                    <td></td>
                                                    <td style="vertical-align:top;"> 
                                                        <img :src="getUserAvatar(message.CreatedUser)" :title="message.CreatedUser" style="width: 20px; height: 20px; border-radius: 50%; " />
                                                    </td>
                                                    <td style="padding-bottom:8px"> 
                                                        <Input v-model="message.Detail"  class="commentContent" type="textarea" :readonly="!canEditComment(message)" :autosize="true" placeholder="Reply..."  @on-blur="updateImproveCommentItem(message)" @on-change="commentItemChanged" />
                                                        <br/>
                                                    </td>
                                                    <td style="vertical-align:top;">
                                                        <a href="#" style="float:right" v-if="canDeleteComment(message)" @click.prevent="deleteImproveComment(message)" title="Delete">
                                                            <span aria-label="Delete" class="">
                                                                <button class="css-b7766g" tabindex="-1" type="button" aria-label="Delete">
                                                                     <i class="fa fa-times fa-1x deleteComment" aria-hidden="true"></i>
                                                                </button>
                                                            </span>
                                                        </a>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td></td>
                                                    <td> 
                                                        <img :src="getUserAvatar(userName)" :title="userName" style="width: 20px; height: 20px; border-radius: 50%; " />
                                                    </td>
                                                    <td> 
                                                        <Input v-model="improve.Comment.Detail" class="commentInputContent" placeholder="Reply..." spellcheck :loading="loading" @on-enter="addImproveComment(improve)" />
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </li>
                                    </ul>
                                </Card>
                            </li>
                        </ul>
                    </td>
                    <td style="vertical-align:top">
                        <ul>
                            <li v-for="action in ActionContent" :key="action.Id">
                                <Card style="width: 100%; text-align: left; margin:0px 0px 3px 0px;">
                                    <!-- background: #ECF5FC -->
                                    <span v-if="action.State == 2" style="float: left; margin-left:5px; color: #CCCCD0">
                                        <i class="fa fa-check-circle fa-1x" style="" aria-hidden="true"></i>
                                        &nbsp;<b>DONE</b>
                                    </span>
                                    <img :src="getUserAvatar(action.CreatedUser)" :title="action.CreatedUser" style="float: right; width: 20px; height: 20px; border-radius: 50%; " />

                                    <Input v-model="action.Detail" :class="getBoardItemClass(3, action.State)" type="textarea" :readonly="!canEditBoardItem(action)" spellcheck :autosize="true" @on-blur="updateBoardItem(action)" @on-change="boardItemChanged" />

                                    <p style="height:22px; ">
                                        <a href="#" @click.prevent="addActionUp(action)" :title="thumbsUpUserNames(action.ThumbsUp)">
                                            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 64px;">
                                                <i :class="thumbsUpClass(action.ThumbsUp)" aria-hidden="true"></i>
                                                &nbsp;<p>{{thumbsUpCount(action.ThumbsUp)}}</p>
                                            </button>
                                        </a>
                                        <!-- <a href="#" @click.prevent="deleteBoardItem(action)" title="Delete" style="float:right" v-if="action.CreatedUser==userName && state != 2">
                                            <Button type="text" class="css-b7766g" tabindex="-1" aria-label="Delete" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 42px;">
                                                <i class="fa fa-trash-o fa-2x" style="color: rgb(239, 83, 80)" aria-hidden="true"></i>
                                            </Button>
                                        </a> -->

                                        <!-- Comments -->
                                        <a v-if="!action.ToggleComment && action.Messages.length==0" href="#" @click.prevent="toggleComment(action)" title="Reply">
                                            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 32px;">
                                                <i :class="replyClass(action.ThumbsUp)" aria-hidden="true"></i>
                                            </button>
                                        </a>
                                         <a v-if="!action.ToggleComment && action.Messages.length > 0" href="#" @click.prevent="toggleComment(action)" :title="thumbsUpUserNames(action.ThumbsUp)">
                                            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 32px;font-size: 8pt;">
                                                <p>{{thumbsUpCount(action.Messages)}} Replies&nbsp;^</p>
                                            </button>
                                        </a>
                                        <a v-if="action.ToggleComment" href="#" @click.prevent="toggleComment(action)" :title="thumbsUpUserNames(action.ThumbsUp)">
                                            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 32px;font-size: 8pt;">
                                                <p>Collapse All&nbsp;^</p>
                                            </button>
                                        </a>

                                        <!-- Menu Items -->
                                        <Dropdown style="float: right;position: relative; font-size:12pt; ">
                                            <Icon type="ios-more" size="28"></Icon>
                                            <DropdownMenu slot="list">
                                              <DropdownItem :disabled="!canDeleteBoardItem(action)"  v-on:click.native="canDeleteBoardItem(action)?deleteBoardItem(action):''"><i class="fa fa-trash-o fa-2x" style="color: rgb(239, 83, 80)" aria-hidden="true"></i>&nbsp;Delete</DropdownItem>
                                              <DropdownItem v-on:click.native="markBoardItem(action, 2)"  v-if="action.State != 2"><i class="fa fa-check-circle fa-2x" style="color: #29984F" aria-hidden="true"></i>&nbsp;Mark as Done</DropdownItem>
                                              <DropdownItem v-on:click.native="markBoardItem(action, 1)"  v-if="action.State != 1"><i class="fa fa-clock-o fa-2x" style="color: #5AC967" aria-hidden="true"></i>&nbsp;Mark as In Progress</DropdownItem>
                                            </DropdownMenu>
                                        </Dropdown>
                                    </p>

                                    <!-- Comments -->
                                    <ul v-if="action.ToggleComment" >
                                        <li>
                                            <table style="width:100%">
                                                  <thead>
                                                    <tr>
                                                        <th width="8%">
                                                        </th>
                                                        <th width="20px">
                                                        </th>
                                                        <th width="92%">
                                                        </th>
                                                        <th width="10px">
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tr v-for="message in action.Messages" :key="message.Id">
                                                    <td></td>
                                                    <td style="vertical-align:top;"> 
                                                        <img :src="getUserAvatar(message.CreatedUser)" :title="message.CreatedUser" style="width: 20px; height: 20px; border-radius: 50%; " />
                                                    </td>
                                                    <td style="padding-bottom:8px"> 
                                                        <Input v-model="message.Detail" class="commentContent" type="textarea" :readonly="!canEditComment(message)" :autosize="true" placeholder="Reply..."  @on-blur="updateActionCommentItem(message)" @on-change="commentItemChanged" />
                                                        <br/>
                                                    </td>
                                                    <td style="vertical-align:top;">
                                                        <a href="#" style="float:right" v-if="canDeleteComment(message)"  @click.prevent="deleteActionComment(message)" title="Delete">
                                                            <span aria-label="Delete" class="">
                                                                <button class="css-b7766g" tabindex="-1" type="button" aria-label="Delete">
                                                                     <i class="fa fa-times fa-1x deleteComment" aria-hidden="true"></i>
                                                                </button>
                                                            </span>
                                                        </a>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td></td>
                                                    <td> 
                                                        <img :src="getUserAvatar(userName)" :title="userName" style="width: 20px; height: 20px; border-radius: 50%; " />
                                                    </td>
                                                    <td> 
                                                        <Input v-model="action.Comment.Detail" class="commentInputContent" placeholder="Reply..." spellcheck :loading="loading" @on-enter="addActionComment(action)" />
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </li>
                                    </ul>
                                </Card>
                            </li>
                        </ul>
                    </td>
                </tr>
            </tbody>
        </table>
        <Table :columns="csvColumns" :data="csvData" ref="tableForExport" v-show="false"></Table>
        <BackTop :height="100" :bottom="120" :right="50">
            <div class="top">Back to Top</div>
        </BackTop>
    </div>
</fullscreen>
</template>

<script>
    const signalR = require("@microsoft/signalr");
    const WentWellType = 1;
    const NeedsImproveType = 2;
    const ActionType = 3;

    const CommentThumbupType = 0;
    const CommentMessageType = 1;

    const AddOperation = 'add';
    const UpdateOperation = 'update';
    const DeleteOperation = 'delete';
    const SortUpOption='asc';
    const SortDownOption='desc';
    const SortCreatedOption='createdTime';
    const SortUserOption="createdUser";

    const DefaultSortButtonClass = 'fa fa-sort fa-1x';
    const DefaultSortButtonTitle = 'Sort Items';

    export default {
        data() {
            return {
                loading: false,
                UserToken: null,
                userName: null,
                ActionContent: null,
                WellContent: null,
                ImproveContent: null,
                boardDetail: {
                    WellDetail: "",
                    ImproveDetail: "",
                    ActionDetail: "",
                },
                boardItemTextChanged: false,
                commentItemTextChanged: false,
                boardName: null,
                boardId: null,
                boardCreatedUser: null,
                connection: "",
                state: 0,
                sortOption:SortUpOption,
                sortButtonClass: DefaultSortButtonClass,
                sortButtonTitle: DefaultSortButtonTitle,
                csvData:[],
                csvColumns: [],
                participants:[],
                showParticipants:false,

                fullscreen: false,
                teleport: true,
                pageOnly: true,
            };
        },
        created() {
            console.log(this.$route.params);
            this.boardId = this.$route.params.boardId;
            this.boardName = this.$route.params.boardName;
            this.boardCreatedUser = this.$route.params.createdUser;
            this.state = this.$route.params.state;
            
            this.UserToken = localStorage.getItem('TOKEN');
            this.userName = localStorage.getItem('LOGINUSER').toUpperCase();
            sessionStorage.setItem('boardDetailPath', JSON.stringify(this.$route.params));
            this.fetchData(true);
            this.init();

            console.log(this.boardCreatedUser+this.userName);
        },
        destroyed(){
            if(this.connection!=null){

                this.updateParticipant(DeleteOperation);

                console.log("Hub for Board Detail with " + this.connection.connectionId + "is stopped");
                this.connection.stop();
            }
        },
        computed: {

        },
        methods: {
            fetchData(forceRefresh) {
                this.fetchBoardItems(forceRefresh);
                this.fetchParticipants();
            },

            fetchBoardItems(forceRefresh) {

                var msg;
                if (forceRefresh) {
                    msg = this.$Message.loading({
                        content: 'Loading Board Items...',
                        duration: 0
                    });
                }

                this.axios(
                    {
                        method: 'get',
                        url: '/BoardItem/' + this.boardId + ''
                       })
                    .then(all => {
                        this.WellContent = all.data.filter(item => item.Type == WentWellType);
                        this.ImproveContent = all.data.filter(item => item.Type == NeedsImproveType);
                        this.ActionContent = all.data.filter(item => item.Type == ActionType);

                        if (forceRefresh) {
                            setTimeout(msg);
                        }
                        return;

                    }).catch(error => {

                        if (forceRefresh) {
                            setTimeout(msg);
                        }
                        console.log('Failed to get board Items. Error:' + error);
                    })
            },
            
            fetchParticipants(){
                this.axios({
                        method: 'get',
                        url: '/User/Online/'+this.boardId
                    }).then((res) => {
                        this.participants = [];
                        res.data.Users.forEach((item) =>{
                            this.participants.push(item.Name);
                        });  
                    }).catch(error=>{
                        this.$Message.error('Failed to load online users. Error:' + error);
                    });
            },
            toggleOnlineUsers(){
                if(this.showParticipants){
                    this.showParticipants = false;
                }
                else {
                    this.showParticipants = true;
                }
            },
            updateParticipant(action){
                
                var context = {
                    Operation: action,
                    UserName: this.userName,
                    BoardId: this.boardId
                };

                this.sendUserMsg(context);

                var opFlag = action == AddOperation ? 1 : 2;
                this.axios({
                        method: 'post',
                        url: '/User/Online',
                        data: {
                            Operation: opFlag,
                            BoardId:this.boardId,
                            Name: this.userName,
                        }
                    }).then(() => {

                    }).catch(error=>{
                        console.log('Error:' + error);
                    });
            },

            updateBoardNameKeydown(event) {
                console.log("test "+event.keyCode);
                if (event.keyCode == 13) {
                    this.updateBoardName();
                    event.preventDefault();
                }
            },
            updateBoardName() {
                if (this.$refs.editBoardName.innerText == this.boardName) {
                    return;
                }
                this.axios({
                    method: 'put',
                    url: '/Board',
                    data: {
                        Id: this.boardId,
                        Name: this.$refs.editBoardName.innerText
                    }
                })
            },

            addBoardDetail(boardDetail, type) {
                if (!boardDetail || !boardDetail.trim()) {
                    return;
                }

                this.loading = true;

                this.axios({
                    method: 'post',
                    url: '/BoardItem',
                    data: {
                        boardid: this.boardId,
                        detail: boardDetail,
                        type: type,
                        createduser: this.userName
                    }
                }).then((res) => {

                    console.log("add");
                    console.log(res.data);
                    var listOfItems = this.getListOfItems(type);
                    listOfItems.unshift(res.data);
                    console.log(res.data);
                    this.renderFunc(boardDetail + ' is created successfully.');

                    var context = {
                        Operation: AddOperation,
                        BoardItem: res.data
                    };
                    this.sendBoardItemMsg(context);

                    this.boardDetail.WellDetail = "";
                    this.boardDetail.ImproveDetail = "";
                    this.boardDetail.ActionDetail = "";

                    this.loading = false;
                }).catch(error => {
                    this.$Message.error('Failed to add board item. Error:' + error);
                    this.loading = false;
                });
            },

            addWentWell() {
                this.addBoardDetail(this.boardDetail.WellDetail, WentWellType);          
            },

            addImproved() {
                this.addBoardDetail(this.boardDetail.ImproveDetail, NeedsImproveType);      
            },

            addAction() {
                this.addBoardDetail(this.boardDetail.ActionDetail, ActionType);
            },
            canEditBoardItem(boardItem){
                if(this.state != 2 && boardItem.CreatedUser== this.userName)
                {
                    return true;
                }

                return false;
            },
            canDeleteBoardItem(boardItem){

                if(this.state != 2 && boardItem.CreatedUser== this.userName)
                {
                    return true;
                }

                return false;
            },
            deleteBoardItem(boardItem) {

                this.$confirm(
                    {
                        message: 'Are you sure delete [' + boardItem.Detail + '] ?',
                        button: {
                            no: 'No',
                            yes: 'Yes'
                        },
                        callback: confirm => {
                            if (confirm) {

                                var listOfItems = this.getListOfItems(boardItem.Type);
                                this.removeBoardItemById(listOfItems, boardItem.Id);

                                this.axios.delete('/BoardItem/' + boardItem.Id + '').then(() => {
                                        console.log(boardItem.Detail + 'is deleted');
                                        this.renderFunc(boardItem.Detail + ' is deleted successfully.');
                                    }).then(() => {
                                        var context = {
                                            Operation: DeleteOperation,
                                            BoardItem: boardItem
                                        };

                                        this.sendBoardItemMsg(context);
                                    })
                            }
                        }
                    }
                )
            },

            getListOfItems(type) {
                var listOfItems;
                if (type == WentWellType) {
                    listOfItems = this.WellContent;
                } else if (type == NeedsImproveType) {
                    listOfItems = this.ImproveContent;
                } else if (type == ActionType) {
                    listOfItems = this.ActionContent;
                }

                return listOfItems;
            },

            findIndexOfBoardItems(arr, id) {
                var index = -1;

                arr.find(function (item, i) {
                    if (item.Id === id) {
                        index = i;
                        return i;
                    }
                });

                return index;
            },

            findBoardItemById(arr, id) {
                var boardItem = null;
                arr.find(function (item) {
                    if (item.Id === id) {
                        boardItem = item;
                        return item;
                    }
                });

                return boardItem;
            },

            removeBoardItemById(arr, id) {
                var index = this.findIndexOfBoardItems(arr, id);
                if (index === -1) {
                    return false;
                }
                return !!arr.splice(index, 1);
            },

            hasNoItemsInTheBoard(){
                return (this.ImproveContent == null || this.ImproveContent.length == 0) && 
                        (this.WellContent == null || this.WellContent.length == 0) &&
                        (this.ActionContent == null || this.ActionContent.length == 0)
            },
            boardItemChanged() {
                this.boardItemTextChanged = true;
            },

            updateBoardItem(boardItem) {

                if (!this.boardItemTextChanged) {
                    return;
                }

                this.axios(
                    {
                        method: 'put',
                        url: '/BoardItem',
                        data: {
                            id: boardItem.Id,
                            detail: boardItem.Detail,
                            type: boardItem.Type,
                            createduser: boardItem.CreatedUser,
                            boardid: this.boardId
                        }
                    }).then(() => {
                        this.boardItemTextChanged = false;
                    }).then(() => {
                        this.renderFunc(boardItem.Detail + ' is updated successfully.');
                    }).then(() => {

                        var context = {
                            Operation: UpdateOperation,
                            BoardItem: boardItem
                        };

                        this.sendBoardItemMsg(context);
                    })
            },

            markBoardItem(boardItem, state) {
                            
                this.axios({
                    method: 'put',
                    url: '/BoardItem',
                    data: {
                        id: boardItem.Id,
                        detail: boardItem.Detail,
                        type: boardItem.Type,
                        state: state,
                        createduser: boardItem.CreatedUser,
                        boardid: this.boardId
                    }
                }).then(() => {
                    boardItem.State = state;
                    this.renderFunc(boardItem.Detail + ' is updated successfully.');
                }).then(() => {
                    var context = {
                        Operation: UpdateOperation,
                        BoardItem: boardItem
                    };              
                    this.sendBoardItemMsg(context);
                })
            },


            addWellUp(wellItem) {
                this.thumbsUpAction(WentWellType, this.WellContent, wellItem, CommentThumbupType);
            },

            addImproveUp(improveItem) {
                this.thumbsUpAction(NeedsImproveType, this.ImproveContent, improveItem, CommentThumbupType);
            },

            addActionUp(actionItem) {
                this.thumbsUpAction(ActionType, this.ActionContent, actionItem, CommentThumbupType);
            },

            thumbsUpAction(type, listOfItems, item, commentType) {
                let username = this.userName;
                var improveItemCache = listOfItems.find(c => c.Id == item.Id);
                var listThrumps = improveItemCache.ThumbsUp;
                var isAdd = true;

                var listItem = listThrumps.find(th => th.CreatedUser == username);
                if (listItem == null) {
                    var likeitem = { BoardItemId: item.Id, CreatedUser: username, Id: 0, DateCreated: null, DateModified: null, Type: commentType };
                    listThrumps.push(likeitem);
                }
                else {
                    var index = listThrumps.findIndex(item => {
                        if (item.CreatedUser == username) {
                            return true;
                        }
                    })
                    listThrumps.splice(index, 1);
                    isAdd = false;
                }

                if (isAdd) {
                    this.addThumps(type, item.Id, commentType);
                }
                else {
                    this.deleteThumps(type, item.Id);
                }
            },

            addWellComment(wellItem){
                this.addComment(WentWellType, this.WellContent, wellItem, CommentMessageType);
            },
            addImproveComment(improveItem){
                this.addComment(NeedsImproveType, this.ImproveContent, improveItem, CommentMessageType);
            },
            addActionComment(actionItem){
                this.addComment(ActionType, this.ActionContent, actionItem, CommentMessageType);
            },

            toggleComment(boardItem)
            {
                boardItem.ToggleComment = !boardItem.ToggleComment;
            },

            addComment(type,listOfItems,boardItem, thumpType) {

                if (!boardItem.Comment.Detail || !boardItem.Comment.Detail.trim()) {
                    return;
                }

                console.log(listOfItems);

                this.axios({
                    method: 'post',
                    url: '/Comment',
                    data: {
                        BoardItemId: boardItem.Id,
                        Type: thumpType,
                        Detail: boardItem.Comment.Detail,
                        CreatedUser: this.userName
                    }
                }).then(res => {
                    var comment = {
                        Id:res.data,
                        CreatedUser: this.userName,
                        Type: thumpType,
                        Detail: boardItem.Comment.Detail,
                        BoardItemId: boardItem.Id
                    };
                    var context = {
                        Operation: AddOperation,
                        BoardId: this.boardId,
                        Type:type,
                        Comment: comment
                    };
                    this.sendCommentMsg(context);

                    boardItem.Comment.Detail = "";
                })
            },

            commentItemChanged() {
                this.commentItemTextChanged = true;
            },

            updateWellCommentItem(message){
                this.updateCommentItem(WentWellType, message);
            },
            updateImproveCommentItem(message){
                this.updateCommentItem(NeedsImproveType, message);
            },
            updateActionCommentItem(message){
                this.updateCommentItem(ActionType, message);
            },

            updateCommentItem(type, comment) {

                if (!this.commentItemTextChanged) {
                    return;
                }

                this.axios(
                    {
                        method: 'put',
                        url: '/Comment',
                        data: {
                            id: comment.Id,
                            detail: comment.Detail,
                            type: comment.Type,
                            createduser: comment.CreatedUser,
                            boarditemid: comment.BoardItemId
                        }
                    }).then(() => {
                        this.commentItemTextChanged = false;
                    }).then(() => {
                        this.renderFunc(comment.Detail + ' is updated successfully.');
                    }).then(() => {

                        var context = {
                            Operation: UpdateOperation,
                            BoardId: this.boardId,
                            Type:type,
                            Comment: comment
                        };
                        this.sendCommentMsg(context);
                    });
            },
            canEditComment(comment){
                if(this.state != 2 && comment.CreatedUser== this.userName)
                {
                    return true;
                }

                return false;
            },

            commentAction(type, listOfItems, item, commentType) {
                let username = this.userName;
                var improveItemCache = listOfItems.find(c => c.Id == item.Id);
                var listThrumps = improveItemCache.ThumbsUp;
                var isAdd = true;

                var listItem = listThrumps.find(th => th.CreatedUser == username);
                if (listItem == null) {
                    var likeitem = { BoardItemId: item.Id, CreatedUser: username, Id: 0, DateCreated: null, DateModified: null, Type: commentType };
                    listThrumps.push(likeitem);
                }
                else {
                    var index = listThrumps.findIndex(item => {
                        if (item.CreatedUser == username) {
                            return true;
                        }
                    })
                    listThrumps.splice(index, 1);
                    isAdd = false;
                }

                if (isAdd) {
                    this.addThumps(type, item.Id, commentType);
                }
                else {
                    this.deleteThumps(type, item.Id);
                }
            },

            getUserAvatar(userName) {
                let userAvatar = ""
                try {
                    userAvatar = require('../assets/Person/' + userName.toLowerCase() + '.jpg')
                    return userAvatar
                } catch (e) {
                    userAvatar = require('../assets/Person/cube.jpg')
                    return userAvatar
                }
            },

            renderFunc(message) {
                this.$Notice.success({
                    //title: 'Notification',
                    desc: 'The desc will hide when you set render.',
                    render: h => {

                        return h('span', [
                            message
                        ])

                        //return h('span', [
                        //    'This is created by ',
                        //    h('a', 'render'),
                        //    ' function'
                        //])
                    }
                });
            },

            init() {
                this.connection = new signalR.HubConnectionBuilder()
                    .withUrl("http://10.63.224.86:5050/BoardHub", {})
                    .configureLogging(signalR.LogLevel.Error)
                    .build();
                this.connection.on("ReceiveBoardItemMessage", boardItemEvent => {
                    if (boardItemEvent.BoardItem.BoardId == this.boardId) {
                        var listOfItems = this.getListOfItems(boardItemEvent.BoardItem.Type);

                        if (boardItemEvent.Operation == DeleteOperation) {
                            this.removeBoardItemById(listOfItems, boardItemEvent.BoardItem.Id);
                        }
                        else if (boardItemEvent.Operation == AddOperation) {
                            console.log('New');
                            console.log(boardItemEvent.BoardItem);
                            var index = this.findIndexOfBoardItems(listOfItems, boardItemEvent.BoardItem.Id);
                            if (index == -1) {
                                listOfItems.unshift(boardItemEvent.BoardItem);
                            }
                        }
                        else if (boardItemEvent.Operation == UpdateOperation) {
                            console.log(boardItemEvent.BoardItem);
                            var boarditemToBeUpdated = this.findBoardItemById(listOfItems, boardItemEvent.BoardItem.Id);
                            if (boarditemToBeUpdated != null) {
                                boarditemToBeUpdated.Detail = boardItemEvent.BoardItem.Detail;
                                boarditemToBeUpdated.State = boardItemEvent.BoardItem.State;
                            }
                        }
                        else {
                            this.fetchData(false);
                        }
                    }
                });

                this.connection.on("ReceiveCommentMessage", commentEvent => {
                    if (commentEvent.BoardId == this.boardId) {
                        var listOfItems = this.getListOfItems(commentEvent.Type);

                        if (commentEvent.Operation == AddOperation) {
                            var improveItemCache1 = listOfItems.find(c => c.Id == commentEvent.Comment.BoardItemId);

                            if(commentEvent.Comment.Type == CommentThumbupType){
                                var listThumbups = improveItemCache1.ThumbsUp;

                                var listItem = listThumbups.find(th => th.CreatedUser == commentEvent.Comment.CreatedUser);
                                if (listItem == null) {
                                    listThumbups.push(commentEvent.Comment);
                                }
                            }
                            else
                            {
                                improveItemCache1.Messages.push(commentEvent.Comment);
                            }
                        }
                        else if (commentEvent.Operation == DeleteOperation) {

                            var improveItemCache2 = listOfItems.find(c => c.Id == commentEvent.Comment.BoardItemId);

                            if(commentEvent.Comment.Type == CommentThumbupType){
                                var listThrumps2 = improveItemCache2.ThumbsUp;
                                var index = listThrumps2.findIndex(item => {
                                    if (item.CreatedUser == commentEvent.Comment.CreatedUser) {
                                        return true;
                                    }
                                });
                                if (index == -1) {
                                    return;
                                }
                                listThrumps2.splice(index, 1);
                            }
                            else
                            {
                                var listComments = improveItemCache2.Messages;
                                var index2 = listComments.findIndex(item => {
                                    if (item.Id == commentEvent.Comment.Id) {
                                        return true;
                                    }
                                });
                                if (index2 == -1) {
                                    return;
                                }

                                listComments.splice(index2, 1);
                            }
                        }
                        else if (commentEvent.Operation == UpdateOperation) {
                            var improveItemCache3 = listOfItems.find(c => c.Id == commentEvent.Comment.BoardItemId);
                            if(commentEvent.Comment.Type == CommentMessageType)
                            {
                                var commentToBeUpdated = improveItemCache3.Messages.find(m=>m.Id == commentEvent.Comment.Id);
                                if (commentToBeUpdated != null) {
                                    commentToBeUpdated.Detail = commentEvent.Comment.Detail;
                                }
                            }
                        }
                    }
                });

                this.connection.on("ReceiveUserMessage", userEvent => {
                    if (userEvent.BoardId == this.boardId) {
                        if(userEvent.UserName != this.userName)
                        {
                            if (userEvent.Operation == AddOperation) {
                                this.$Notice.info({
                                    render: h => {
                                            return h('span', [
                                                userEvent.UserName + ' joined the board.'
                                            ])}
                                });

                                var index = this.participants.indexOf(userEvent.UserName);
                                if(index == -1)
                                {
                                    this.participants.push(userEvent.UserName);
                                }
                            }
                            else if(userEvent.Operation == DeleteOperation){
                                this.$Notice.info({
                                    render: h => {
                                            return h('span', [
                                                userEvent.UserName + ' left the board.'
                                            ])}
                                });

                                var index2 = this.participants.findIndex(item => {
                                                if (item == userEvent.UserName) {
                                                    return true;
                                                }
                                            });
                                if (index2 == -1) {
                                    return;
                                }
                                this.participants.splice(index2, 1);
                            }
                        }
                    }
                });

                this.connection.start().then(() => {
                     var index = this.participants.indexOf(this.userName);
                    if(index == -1)
                    {
                        this.participants.push(this.userName);
                    }

                    this.updateParticipant(AddOperation);
                });
            },

            sendBoardMsg() {
                this.connection.invoke("SendBoardMessage");
            },

            sendBoardItemMsg(context) {
                this.connection.invoke("SendBoardItemMessage", context);
            },

            sendCommentMsg(context) {
                this.connection.invoke("SendCommentMessage", context);
            },

            sendUserMsg(context) {
                this.connection.invoke("SendUserMessage", context);
            },

            thumbsUpUserNames(thumbsUp) {
                var names = ''
                for (var i = 0; i < thumbsUp.length; i++) {
                    names = names + thumbsUp[i].CreatedUser +'\n';
                }
                return names;
            },

            thumbsUpCount(thumbsUp) {

                var len = thumbsUp.length;
                if(len>0)
                {
                    return len;
                }
                
                return '';
            },

            thumbsUpClass(thumbsUp){
                var style='fa fa-2x';
                var index = thumbsUp.find(item=> item.CreatedUser == this.userName);
                if(index!=null)
                {
                    style += ' fa-thumbs-up';
                }
                else
                {
                     style += ' fa fa-thumbs-o-up';
                }

                if(this.thumbsUpCount(thumbsUp) >0 )
                {
                    style += ' commentActionHighlightStyle';
                }
                else
                {
                    style += ' commentActionStyle';
                }
                return style;
            },

            commentContentClass(message)
            {
                var style='';
                if(message.CreatedUser == this.userName)
                {
                    style = 'commentContentHighlight';
                }
                else
                {
                    style = 'commentContent';
                }
                return style;
            },

            replyClass(thumbsUp){
                var index = thumbsUp.find(item=> item.CreatedUser == this.userName);
                if(index!=null)
                {
                    return 'fa fa-comment-o fa-2x commentActionStyle';
                }
                else
                {
                    return 'fa fa-comment-o fa-2x commentActionStyle';
                }
            },

            addThumps(type,boardItemId,thumpType) {
                this.axios({
                    method: 'post',
                    url: '/Comment',
                    data: {
                        BoardItemId: boardItemId,
                        Type: thumpType,
                        CreatedUser: this.userName
                    }
                }).then(() => {
                    var comment = {
                        CreatedUser: this.userName,
                        Type: thumpType,
                        BoardItemId: boardItemId
                    };
                    var context = {
                        Operation: AddOperation,
                        BoardId: this.boardId,
                        Type:type,
                        Comment: comment
                    };
                    this.sendCommentMsg(context);
                })
            },
            deleteThumps(type, boardItemId) {
                this.axios({
                    method: 'delete',
                    url: '/Comment',
                    params: {
                        borderItemId: boardItemId,
                        username: this.userName
                    }
                }).then(() => {
                    var comment = {
                        CreatedUser: this.userName,
                        Type: 0,
                        BoardItemId: boardItemId
                    };
                    var context = {
                        Operation: DeleteOperation,
                        BoardId: this.boardId,
                        Type: type,
                        Comment: comment
                    };
                    this.sendCommentMsg(context);
                })
            },

            canDeleteComment(message){

                if(this.state != 2 && message.CreatedUser == this.userName)
                {
                    return true;
                }

                return false;
            },

            deleteWellComment(message) {
                this.deleteComment(WentWellType, message);
            },

            deleteImproveComment(message) {
                this.deleteComment(NeedsImproveType, message);
            },

            deleteActionComment(message) {
                this.deleteComment(ActionType, message);
            },
            deleteComment(type, message)
            {
                this.$confirm(
                    {
                        message: 'Are you sure delete comment [' + message.Detail + '] ?',
                        button: {
                            no: 'No',
                            yes: 'Yes'
                        },
                        callback: confirm => {
                            if (confirm) {
                                this.axios.delete('/Comment/' + message.Id + '')
                                            .then(() => {
                                                var context = {
                                                    Operation: DeleteOperation,
                                                    BoardId: this.boardId,
                                                    Type: type,
                                                    Comment: message
                                                };
                                                this.sendCommentMsg(context);
                                            })
                            }
                        }
                    })
            },
            markCompleted() {
                this.$confirm(
                    {
                        message: 'Are you sure mark board [' + this.boardName +"] as Completed ? after that, you can't edit this board again",
                        button: {
                            no: 'No',
                            yes: 'Yes'
                        },
                        callback: confirm => {
                            if (confirm) {
                                this.axios({
                                    method: 'put',
                                    url: '/Board',
                                    data: {
                                        id: this.boardId,
                                        name: this.boardName,
                                        state: 2,
                                        createduser: this.userName
                                    }
                                }).then(() => {
                                    this.state = 2;
                                    this.renderFunc(this.boardName + ' is marked as completed successfully.');
                                }).then(() => {
                                    this.sendBoardItemMsg();
                                })
                            }
                        }
                    }
                )
            },

            deleteBoard() {
                this.$confirm(
                    {
                        message: 'Are you sure delete board [' + this.boardName +'] ?',
                        button: {
                            no: 'No',
                            yes: 'Yes'
                        },
                        callback: confirm => {
                            if (confirm) {
                                this.axios.delete('/Board/' + this.boardId + '').then(() => {
                                        this.renderFunc(this.boardName + ' is deleted successfully.');
                                    }).then(() => {
                                        this.sendBoardMsg();
                                    }).then(() => {
                                        this.$router.push('/boardAll');
                                    })
                            }
                        }
                    }
                )
            },
            exportData() {
                this.csvColumns = [
                {
                    "title": "",
                    "key": "content",
                },
                {
                    "title": "",
                    "key": "createdUser",
                },
                {
                    "title": "",
                    "key": "vote",
                }];

                this.csvData.push({ "content": this.boardName, "createdUser":"", "vote": "" });
                this.csvData.push({ "content": "", "createdUser":"",  "vote": "" });
                this.csvData.push({ "content": "Went Well", "createdUser":"Submitter",  "vote": "Votes" });
                this.getSortedItems(this.WellContent).forEach((value,index) => {
                    this.csvData.push({ "content": index+1 +'.'+ value.content, "createdUser":value.createdUser, "vote": value.vote });
                });

                this.csvData.push({ "content": "", "createdUser":"","vote": "" });
                this.csvData.push({ "content": "To Improve", "createdUser":"Submitter", "vote": "Votes" });
                this.getSortedItems(this.ImproveContent).forEach((value,index) => {
                    this.csvData.push({ "content": index+1 +'.'+ value.content, "createdUser":value.createdUser, "vote": value.vote });
                });

                this.csvData.push({ "content": "", "createdUser":"", "vote": "" });
                this.csvData.push({ "content": "Action Items", "createdUser":"Submitter", "vote": "Votes" });
                this.getSortedItems(this.ActionContent).forEach((value,index) => {
                    this.csvData.push({ "content": index+1 +'.'+ value.content, "createdUser":value.createdUser, "vote": value.vote });
                });

                this.$refs.tableForExport.exportCsv({
                    filename: this.boardName,
                    columns: this.csvColumns,
                    data: this.csvData
                })
            },
            getSortedItems(content)
            {
                let sortedItems = [];
                content.forEach((value) => {
                    sortedItems.push({ 
                        "content": value.Detail.replaceAll(',','_').replaceAll('\n','').replaceAll('\r',''), 
                        "createdUser": value.CreatedUser, 
                        "vote": value.ThumbsUp.length 
                        });
                });

                sortedItems.sort(function(a, b) {
                    return b.vote-a.vote
                });

                return sortedItems;
            },
            sortItems()
            {
                if(this.sortOption == SortUpOption)
                {
                    this.sortItemsAsc(this.WellContent);
                    this.sortItemsAsc(this.ImproveContent);
                    this.sortItemsAsc(this.ActionContent);
                    this.sortButtonClass = "fa fa-sort-amount-desc fa-1x";
                    this.sortButtonTitle = "Sorted Items by Thumbup Descending";

                    this.sortOption = SortDownOption;               
                }
                else if(this.sortOption == SortDownOption)
                {
                    this.sortItemsDesc(this.WellContent);
                    this.sortItemsDesc(this.ImproveContent);
                    this.sortItemsDesc(this.ActionContent);
                     this.sortButtonClass = "fa fa-sort-amount-asc fa-1x";
                     this.sortButtonTitle = "Sorted Items by Thumbup Ascending";

                     this.sortOption = SortUserOption;
                }
                else if(this.sortOption == SortUserOption)
                {
                    this.sortItemsUserAsc(this.WellContent);
                    this.sortItemsUserAsc(this.ImproveContent);
                    this.sortItemsUserAsc(this.ActionContent);
                    this.sortButtonClass = "fa fa-sort-alpha-asc fa-1x";
                    this.sortButtonTitle = "Sorted Items by User Name Ascending";

                    this.sortOption = SortCreatedOption;

                }
                else if(this.sortOption == SortCreatedOption)
                {
                    this.resetSortItems(this.WellContent);
                    this.resetSortItems(this.ImproveContent);
                    this.resetSortItems(this.ActionContent);
                    this.sortButtonClass = DefaultSortButtonClass;
                    this.sortButtonTitle = DefaultSortButtonTitle;

                     this.sortOption = SortUpOption;
                }
            },
            sortItemsAsc(content)
            {
                content.sort(function(a, b){
                    return b.ThumbsUp.length - a.ThumbsUp.length;
                });
            },
            sortItemsDesc(content)
            {
                content.sort(function(a, b){
                    return a.ThumbsUp.length - b.ThumbsUp.length;
                });
            },
            sortItemsUserAsc(content)
            {
                content.sort(function(a, b){
                    return a.CreatedUser.localeCompare(b.CreatedUser);
                });
            },
            resetSortItems(content)
            {
                content.sort(function(a, b){
                    return  b.Id - a.Id;
                });
            },
            getBoardItemClass(type, state)
            {
                var style = 'boardItemContent';
                if(state == 2)
                {
                    return style + ' ' + 'boardItemDoneClass';
                }

                if(type == NeedsImproveType)
                {
                    return style + ' ' + 'improveItem';
                }
                else if(type == ActionType)
                {
                    return style + ' ' + 'actionItem';
                }
            },
            toggle () {
                this.fullscreen = !this.fullscreen
            },
        }
    }
</script>

<style>
    @import "../styles/css/font-awesome.css";

    ul {
        list-style-type: none;
    }

    textarea {
        border-style: none !important;
    }

    .boardItemContent {
        margin-bottom: 15px;
    }
    .boardItemContent .ivu-input{
        background-color:transparent;
        font-family: -apple-system,BlinkMacSystemFont,Segoe UI,Roboto,Oxygen,Ubuntu,Fira Sans,Droid Sans,Helvetica Neue,sans-serif;
        font-size: 12pt;
    }

    .wellItem  .ivu-input{
        color: #50AA62;
    }
    .improveItem  .ivu-input{
        color: #E8897A;
    }
    .actionItem  .ivu-input{
        color: #7A93E8;
    }

    .boardItemDoneClass .ivu-input{
        text-decoration: line-through;
        font-style: oblique;
        /* color: #218338; */
        /* color:#29984F; */
        color: #CCCCD0;
    }

    .commentUserName{
        font-size: 6pt; 
        color:#CDC7C7;
        padding-left: 5px;
    }

    .commentContent .ivu-input{
        background-color:#FFFFFF;
        /* margin-bottom: 200x; */
        font-size: 11pt;
    }

    .commentContentHighlight .ivu-input{
        background-color:#F6FEFE;
        /* margin-bottom: 200x; */
        font-size: 11pt;
    }

    .commentActionStyle{
        color: #E8F5E8
    }

    .commentActionStyle:hover {
        color: #4CAF50
    }

    .commentActionHighlightStyle{
        color: #4CAF50
    }

    .deleteComment{
        color: #F8F8F8
    }

    .deleteComment:hover {
        color: #E87D7D
    }

    .commentInputContent .ivu-input{
        background-color:#FFFFFF;
        font-size: 11pt;
    }

    .wellInputContent .ivu-input{
        background-color:#F2F5F2;
        font-size: 11pt;
    }

    .improveInputContent .ivu-input{
        background-color:#FBF5F5;
        font-size: 11pt;
    }

    .actionInputContent .ivu-input{
        background-color:#ECF5FC;
        font-size: 11pt;
    }

    .css-vubbuv {
        user-select: none;
        width: 1em;
        height: 1em;
        display: inline-block;
        fill: currentcolor;
        flex-shrink: 0;
        transition: fill 200ms cubic-bezier(0.4, 0, 0.2, 1) 0ms;
        font-size: 1.5rem;
    }

    .css-b7766g {
        display: inline-flex;
        -webkit-box-align: center;
        align-items: center;
        -webkit-box-pack: center;
        justify-content: center;
        position: relative;
        box-sizing: border-box;
        -webkit-tap-highlight-color: transparent;
        background-color: transparent;
        outline: 0px;
        border: 0px currentcolor;
        margin: 0px;
        cursor: pointer;
        user-select: none;
        vertical-align: middle;
        appearance: none;
        text-decoration: none;
        font-family: Roboto, Helvetica, Arial, sans-serif;
        font-weight: 500;
        font-size: 0.875rem;
        line-height: 1.75;
        letter-spacing: 0.02857em;
        /* text-transform: uppercase; */
        min-width: 8px;
        border-radius: 4px;
        transition: background-color 250ms cubic-bezier(0.4, 0, 0.2, 1) 0ms, box-shadow 250ms cubic-bezier(0.4, 0, 0.2, 1) 0ms, border-color 250ms cubic-bezier(0.4, 0, 0.2, 1) 0ms, color 250ms cubic-bezier(0.4, 0, 0.2, 1) 0ms;
        color: inherit;
    }

    .top {
        padding: 10px;
        background: rgba(0, 153, 229, .7);
        color: #fff;
        text-align: center;
        border-radius: 5px;
    }

    .noItemsStyle{
        width: 100%; 
        text-align: center; 
        font-size: 12pt;
        color:#D1D0CE;
        margin-top: 250px;
    }

    [contenteditable]:focus {
        outline: none;
        border: 2px solid rgba(0, 153, 229, .7);
        border-radius: 4px;
        transition: border .2s ease-in-out,background .2s ease-in-out,box-shadow .2s ease-in-out;
    }

    .subLayout{
        minHeight:300px;height:100%;width:100%;padding:12px;position:absolute;overflow-y:auto;background:#FFFFFF;
    }

    .layoutDetail .ivu-card-body {
        padding: 16px;
    }

</style>