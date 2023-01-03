<script>
export default {
  data() {
    return {
      history: [
        { 
            "Date": '2023-01-01', 
            "Version": "Version 1.0", 
            "Desc": "Support Multiple Projects",
            "IsNew": true
        },
        { 
            "Date": '2022-12-28', 
            "Version": "Version 0.9", 
            "Desc": "Support Completion Rate of Action and Assignee" ,
            "IsNew": true
        },
        { 
            "Date": '2022-10-24', 
            "Version": "Version 0.8", 
            "Desc": "Show Statistics data of User" 
        },
        { 
            "Date": '2022-09-03', 
            "Version": "Version 0.7", 
            "Desc": "Associate What can be improved with Action" 
        },
        { 
            "Date": '2022-04-24', 
            "Version": "Version 0.6", 
            "Desc": "Support Comment Feature" 
        },
        { 
            "Date": '2022-04-16', 
            "Version": "Version 0.5", 
            "Desc": "Support Full Screen, Sort Items by User" 
        },
        { 
            "Date": '2022-03-22', 
            "Version": "Version 0.4", 
            "Desc": "Show online users, add navigation bar" 
        },
        { 
            "Date": '2022-03-18', 
            "Version": "Version 0.3", 
            "Desc": "Support Spellcheck, Sort Items by Votes" 
        },
        { 
            "Date": '2022-03-13', 
            "Version": "Version 0.2", 
            "Desc": "Export to CSV, Filter Boards by status,Fix bugs" 
        },
        { 
            "Date": '2022-03-01', 
            "Version": "Version 0.1", 
            "Desc": "Release the first version of Cube" 
        }
      ]
    };
  },
  created(){
  }
}
</script>

<template>
  <div>
        <Row v-for="item in history" :key="item">
            <Col span="4">{{ item.Date }}</Col>
            <Col span="4">{{ item.Version }}</Col>
            <Col span="16">
              {{  item.Desc }}
              <Badge v-if="item.IsNew" text="new">      
                &nbsp;&nbsp;&nbsp;&nbsp;
              </Badge>
            </Col>
        </Row>
  </div>
</template>
