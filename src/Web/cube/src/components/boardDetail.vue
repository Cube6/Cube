<template>
<fullscreen v-model="fullscreen" :teleport="teleport" :page-only="pageOnly">
    <div class="subLayout">
        <h1 style="width:100%;text-align:center; font-size:larger">
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
                <img v-for="user in participants" :hidden='showParticipants==false'  :key="user" :src="getUserAvatar(user)" :title="user" style="float: left; width: 20px; height: 20px; border-radius: 50%; " />
            </span>
        </h1>
        <br />
        <table width="100%" height="100%" @click="cleanFocusedImprovedItem(currentFocusImprovedItemId)">
            <tr style="vertical-align:top">
                <td width="33%">
                    <table width="100%">
                        <thead>
                            <tr v-if="state == 2">
                                <th width="100%">
                                    What went well ?
                                </th>
                            </tr>
                            <tr v-if="state != 2">
                                <th width="100%">
                                    <Input element-id="wentWell" v-model="boardDetail.WellDetail" class="wellInputContent" type="textarea" :autosize="{maxRows: 4}" placeholder="What went well ?" spellcheck :loading="loading" @keydown.native="handleKeydown" />
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td style="vertical-align:top">
                                    <ul>
                                        <li v-for="well in WellContent" :key="well.Id">
                                            <Card style="width: 100%; text-align: left; margin:0px 0px 3px 0px;" @click="clickContent" > 
                                                <!-- background: #F1F3F1 -->
                                                <img :src="getUserAvatar(well.CreatedUser)" :title="well.CreatedUser" style="width: 20px; height: 20px; border-radius: 50%; margin-bottom: 5px;" />

                                                <!-- Menu Items -->
                                                <Dropdown style="float: right;position: relative; font-size:12pt; ">
                                                    <Icon type="ios-more" size="28"></Icon>
                                                    <DropdownMenu slot="list">
                                                    <DropdownItem :disabled="!canDeleteBoardItem(well)"  v-on:click.native="canDeleteBoardItem(well)?deleteBoardItem(well):''"><i class="fa fa-trash-o fa-2x" style="color: rgb(239, 83, 80)" aria-hidden="true"></i><span style="color:red">&nbsp;Delete</span></DropdownItem>
                                                    </DropdownMenu>
                                                </Dropdown>

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
                            </tr>
                        </tbody>
                    </table>
                </td>
               <td width="33%">
                     <table width="100%">
                        <thead>
                            <tr v-if="state == 2">
                                <th width="100%">
                                    What could be improved ?
                                </th>
                            </tr>
                            <tr v-if="state != 2">
                                <th width="100%">
                                    <Input element-id="improved" v-model="boardDetail.ImproveDetail" class="improveInputContent" type="textarea" :autosize="{maxRows: 4}" placeholder="What could be improved ?" spellcheck @keydown.native="handleKeydown" />
                                </th>
                            </tr>
                        </thead>
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
                                            <Card :style="{'opacity': setImprovedItemOpacity(improve.Id)}" style="width: 100%; text-align: left; margin:0px 0px 3px 0px;" @click.native.stop="refocusImprovedItem(improve.Id)">
                                                <!-- background: #FBF5F5 -->
                                                
                                                <img :src="getUserAvatar(improve.CreatedUser)" :title="improve.CreatedUser" style="float: left;width: 20px; height: 20px; border-radius: 50%; margin-bottom: 5px; margin-left: 5px;" />
                                               

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
                                                    
                                                    <a href="#" @click.prevent.stop="showAllAssociatedActions(improve.Id)" title="View Actions">
                                                        <button class="css-b7766g" tabindex="-1" style="float: right; position: relative; padding-left: 0px; padding-right: 0px; min-width: 32px;">
                                                            <i :class="getViewActionsClass(improve.Id)" aria-hidden="true"></i>
                                                        </button>
                                                    </a>
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
                                                                    <Input v-model="message.Detail" :class="commentContentClass(message)" type="textarea" :readonly="!canEditComment(message)" :autosize="true" placeholder="Reply..."  @on-blur="updateImproveCommentItem(message)" @on-change="commentItemChanged" />
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
                            </tr>
                        </tbody>
                    </table>
                </td>
                <td width="34%">  
                    <table width="100%">
                        <thead>
                            <tr v-if="state == 2">
                                <th width="100%">
                                    Action Items
                                </th>
                            </tr>
                            <tr v-if="state != 2">
                                <th width="100%">
                                    <Input element-id="action" v-model="boardDetail.ActionDetail" class="actionInputContent" type="textarea" :autosize="{maxRows: 4}" placeholder="Action Items" spellcheck @keydown.native="handleKeydown" />
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td style="vertical-align:top">
                                    <ul>
                                        <li v-for="action in ActionContent" :key="action.Id">
                                            <transition name="transition-drop">
                                                <Card v-show="currentFocusImprovedItemId == null || currentFocusImprovedItemId == action.AssociatedBoardItemId" style="width: 100%; text-align: left; margin:0px 0px 3px 0px;" @click.native.stop="focusAssociatedImprovedItem(action.AssociatedBoardItemId)">
                                                    <!-- background: #ECF5FC -->
                                                    <img :src="getUserAvatar(action.CreatedUser)" :title="action.CreatedUser" style="float: left; width: 20px; height: 20px; border-radius: 50%; margin-bottom: 5px;" />
                                                    
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

                                                    <p style="height:22px;">
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
                                                                        <Input v-model="message.Detail" :class="commentContentClass(message)" type="textarea" :readonly="!canEditComment(message)" :autosize="true" placeholder="Reply..."  @on-blur="updateActionCommentItem(message)" @on-change="commentItemChanged" />
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