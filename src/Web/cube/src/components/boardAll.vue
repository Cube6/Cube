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
              <BoardCard :board="board" v-on:click.native="ViewBoard(board)"/>
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