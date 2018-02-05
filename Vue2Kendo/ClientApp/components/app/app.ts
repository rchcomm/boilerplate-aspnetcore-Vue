import Vue from 'vue';
import { Component } from 'vue-property-decorator';

//import '@progress/kendo-ui'
//import '@progress/kendo-theme-default/dist/all.css'

//import { Grid, GridInstaller } from '@progress/kendo-grid-vue-wrapper'

//Vue.use(GridInstaller)

@Component({
    components: {
        MenuComponent: require('../navmenu/navmenu.vue.html')
        //, Grid
    }
})
export default class AppComponent extends Vue {
    
}
