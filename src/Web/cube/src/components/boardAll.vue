<template>
    <div class="subLayout">
        <div class="main-board">
            <ul>
                <li style="width:260px; float: left;">
                    <Card style="width: 250px; cursor: pointer;" v-on:click.native="AddBoard()">
                        <div style="display: flex; padding-bottom: 36px; background: #fff;">
                            <i scripturl="../scripts/font.js" style="font-size:24px;">
                                <svg viewBox="0 0 1024 1024" style="width:1em;height:1em;fill:currentColor;overflow:hidden">
                                    <use xlink:href="#at-plus"></use>
                                </svg>
                            </i>
                        </div>
                        <span style="position:absolute;left:16px;bottom:12px;">Add Board</span>
                    </Card>
                </li>
                <li v-for="board in post" :key="board.Id" style="width:260px; float: left;">
                    <Card :style="{width: '250px', cursor: 'pointer', margin:'0px 0px 15px 0px', background: getBoardCardBG(board)}" v-on:click.native="ViewBoard(board)">
                        <p slot="title" :title="getBoardCardTooltip(board)">
                            <span style="float:right">
                                <img :src="getUserAvatar(board.CreatedUser)" :title="'Owner: ' + board.CreatedUser" style="width:20px; height:20px; border-radius:50%; " />
                            </span>
                        <span>
                                <img v-if="board.State == 2" src="../assets/Icons/completed.jpg" title="Completed" style="width:20px; height:20px; border-radius:50%;" >           
                                <img v-if="board.IsDeleted==false && board.State == 1" src="../assets/Icons/inprogress.jpg" title="In Progress" style="width:20px; height:20px; border-radius:50%; " >
                                <img v-if="board.IsDeleted" src="../assets/Icons/deleted.png" title="Deleted" style="width:20px; height:20px; border-radius:50%;" >
                            </span>
                        </p>
                        <div style="text-align: center; overflow: hidden; white-space: nowrap; text-overflow: ellipsis;">
                            <a href="#" slot="extra" :title="getBoardCardTooltip(board)">
                                {{board.Name}}
                            </a>
                        </div>
                        <!-- <a href="#" slot="extra" @click.prevent="DeleteBoard(board)" title="Delete" v-if="board.CreatedUser==UserName">
                            <span aria-label="Delete" class="">
                                <button class="css-b7766g" tabindex="-1" type="button" aria-label="Delete" style="position: relative; padding-left: 0px; padding-right: 0px; min-width: 42px;">
                                    <svg class="css-vubbuv" focusable="false" viewBox="0 0 24 24" aria-hidden="true" style="color: rgb(239, 83, 80);">
                                        <use xlink:href="#at-delete"></use>
                                    </svg>
                                </button>
                            </span>
                        </a> -->
                    </Card>
                </li>
            </ul>
        </div>
        <div class="page-footer">
            <Page :total="pageSetting.total" :page-size="pageSetting.pageSize" :current="pageSetting.currentPage" @on-change="pageChanged" />
        </div>
    </div>
</template>

<script>
	import boardAll from "../scripts/boardAll.js";
	export default boardAll;
</script>

<style>
	@import "../styles/board.css";
</style>