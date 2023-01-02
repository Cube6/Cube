<script>
    import comment from "./CommentList.js";
    export default comment;
</script>

<template>
  <div>
    <p style="height:22px;">
        <Tooltip :content="thumbsUpUserNames(boardItem.ThumbsUp)" placement="bottom">
            <a href="#" @click.prevent="addWellUp(boardItem)">
                <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 64px;">
                    <i :class="thumbsUpClass(boardItem.ThumbsUp)" aria-hidden="true"></i>
                    &nbsp;<p>{{thumbsUpCount(boardItem.ThumbsUp)}}</p>
                </button>
            </a>
            <template #content>
                <p v-for="thumbsUp in boardItem.ThumbsUp" :key="thumbsUp.Id">
                    {{ thumbsUp.CreatedUser }}
                </p>
            </template>
        </Tooltip>

        <!-- Comments -->
        <Tooltip content="Comment" placement="bottom">
            <a v-if="!boardItem.ToggleComment && boardItem.Messages.length==0" href="#" @click.prevent="toggleComment(boardItem)">
                <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 32px;">
                    <i :class="replyClass(boardItem.ThumbsUp)" aria-hidden="true"></i>
                </button>
            </a>
        </Tooltip>
        <a v-if="!boardItem.ToggleComment && boardItem.Messages.length > 0" href="#" @click.prevent="toggleComment(boardItem)">
            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 32px;font-size: 8pt;">
                <p>{{thumbsUpCount(boardItem.Messages)}} Comments&nbsp;^</p>
            </button>
        </a>
        <a v-if="boardItem.ToggleComment" href="#" @click.prevent="toggleComment(boardItem)">
            <button class="css-b7766g" tabindex="-1" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 32px;font-size: 8pt;">
                <p>Collapse All&nbsp;^</p>
            </button>
        </a>
    </p>
    <ul v-if="boardItem.ToggleComment" >
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
               <tr v-for="message in boardItem.Messages" :key="message.Id">
                   <td></td>
                   <td style="vertical-align:top;"> 
                       <img :src="getUserAvatar(message.CreatedUser)" :title="message.CreatedUser" style="width: 24px; height: 24px; border-radius: 50%; " />
                   </td>
                   <td style="padding-bottom:8px"> 
                       <span class="commentUserName">
                           {{message.CreatedUser}}
                           <Time style="float:right" :time="(new Date(message.DateCreated)).getTime() +8*60*60*1000 " />
                       </span>
                       <Input v-model="message.Detail" :class="commentContentClass(message)" type="textarea" :readonly="!canEditComment(message)" :autosize="true" placeholder="Add a new comment"  @on-blur="updateCommentItem(message)" @on-change="commentItemChanged" />
                       <br/>            
                   </td>
                   <td style="vertical-align:top;">
                       <a href="#" style="float:right" v-if="canDeleteComment(message)" @click.prevent="deleteCommentItem(message)" title="Delete">
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
                       <img :src="getUserAvatar(userName)" :title="userName" style="width: 24px; height: 24px; border-radius: 50%; " />
                   </td>
                   <td> 
                       <Input v-model="boardItem.Comment.Detail" class="commentInputContent" placeholder="Add a new comment" spellcheck :loading="loading" @on-enter="addCommentItem(boardItem)" />
                   </td>
                   <td></td>
               </tr>
           </table>
       </li>
    </ul>
  </div>
</template>
