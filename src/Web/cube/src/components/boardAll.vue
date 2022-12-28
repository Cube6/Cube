<template>
    <div class="subLayout">
        <div class="main-board">
          <ul>
            <li style="width:260px; float: left;">
              <Card style="width: 250px; cursor: pointer;" v-on:click.native="AddBoard()">
                <div style="display: flex; padding-bottom: 36px; background: #fff; height:113px">
                  <i scripturl="../scripts/font.js" style="font-size:25px;">
                    <svg viewBox="0 0 1024 1024" style="width:1em;height:1em;fill:currentColor;overflow:hidden">
                      <use xlink:href="#at-plus"></use>
                    </svg>
                  </i>
                </div>
                <span style="position:absolute;left:50px;bottom:50px; font-size:13pt;">Create a new board</span>
              </Card>
            </li>
            <li v-for="board in showBoardData" :key="board.Id" style="width: 260px; float: left;padding-right: 10px">
              <Card :style="{background:getBoardCardFullBG(board), width: '250px', cursor: 'pointer',float: 'right'}" v-on:click.native="ViewBoard(board)">
                <p slot="title" style="height:25px;">
                  <span style="float:right">
                    <Tooltip :content="'Owner: ' + board.CreatedUser" placement="bottom">
                      <img :src="getUserAvatar(board.CreatedUser)" style="width:24px; height:24px; border-radius:50%; " />
                    </Tooltip>
                  </span>
                  <span>
                    <img v-if="board.State == 2" src="../assets/Icons/completed.jpg" title="Completed" style="width:20px; height:20px; border-radius:50%;">
                    <img v-if="board.IsDeleted==false && board.State == 1" src="../assets/Icons/inprogress.jpg" title="In Progress" style="width:20px; height:20px; border-radius:50%; ">
                    <img v-if="board.IsDeleted" src="../assets/Icons/deleted.png" title="Deleted" style="width:20px; height:20px; border-radius:50%;">
                  </span>
                </p>
                <div style="text-align: center; overflow: hidden; text-overflow: ellipsis; height:38px">
                  <a href="#" slot="extra">
                    {{board.Name}}
                  </a>
                </div>
                <div style="float:right; height: 30px;">
                  <Progress vertical :percent="getCompletionRateOfAction(board)" :stroke-width="5" title="Completion rate of Action"/>
                </div>
                <div style="text-align: center; overflow: hidden; color: #b7beb7; white-space: nowrap; text-overflow: ellipsis;">
                  <!-- {{formatBoardCreateTime(board)}} -->
                  <Time :time="(new Date(board.DateCreated)).getTime() +8*60*60*1000 " />
                </div>
              </Card>
              <Card :style="{ width: '5px', height: '150px','background-color': getBoardCardBG(board)}"></Card>
            </li>
          </ul>
        </div>
        <div class="page-footer">
            <Page :total="pageSetting.total" :page-size="pageSetting.pageSize" 
                  :current="pageSetting.currentPage" 
                  @on-change="pIndexChange" @on-page-size-change="pSizeChange" 
                  prev-text="Previous" next-text="Next" 
                  show-elevator show-sizer />
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