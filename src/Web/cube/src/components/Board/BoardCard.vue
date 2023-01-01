<script>
export default {
  props: ['board'],
  data() {
    return {
    };
  },
  created(){
    console.log(this.board);
  },
  methods:{
    getBoardCardFullBG(board) {

      if (board.IsDeleted == false && board.State == 1) {
        return 'rgb(238,255,242)';
      }
      return '#FFFFFF';
    },
    getUserAvatar(userName) {
      let userAvatar = ""
      try {
        userAvatar = require('../../assets/Person/' + userName.toLowerCase() + '.jpg')
        return userAvatar
      } catch (e) {
        userAvatar = require('../../assets/Person/cube.jpg')
        return userAvatar
      }
    },
    getCompletionRateOfAction(board) {
      var total = board.BoardItems.length;
      if(total==0)
      {
        return 100;
      }
      var numOfComplete = board.BoardItems.filter(c => c.Type == 3 && c.State == 2).length;

      var percent = (numOfComplete/total)*100;
      return Math.round(percent);
    },
    getBoardCardBG(board) {
      if (board.IsDeleted == false && board.State == 1) {
        return 'rgb(79 201 13)';
      }
      // else if(board.State == 2)
      // {
      //     return '#7FEE7A';
      // }
      else if (board.IsDeleted) {
        return '#f3c1c1';
      }
      return '#c4ebaf';
    },
  }
}
</script>

<template>
  <div>
    <Card :style="{background:getBoardCardFullBG(board), width: '250px', cursor: 'pointer',float: 'right'}">
        <p slot="title" style="height:25px;">
          <span style="float:right">
            <Tooltip :content="'Owner: ' + board.CreatedUser" placement="bottom">
              <img :src="getUserAvatar(board.CreatedUser)" style="width:24px; height:24px; border-radius:50%; " />
            </Tooltip>
          </span>
          <span>
            <img v-if="board.State == 2" src="../../assets/Icons/completed.jpg" title="Completed" style="width:20px; height:20px; border-radius:50%;">
            <img v-if="board.IsDeleted==false && board.State == 1" src="../../assets/Icons/inprogress.jpg" title="In Progress" style="width:20px; height:20px; border-radius:50%; ">
            <img v-if="board.IsDeleted" src="../../assets/Icons/deleted.png" title="Deleted" style="width:20px; height:20px; border-radius:50%;">
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
  </div>
</template>
