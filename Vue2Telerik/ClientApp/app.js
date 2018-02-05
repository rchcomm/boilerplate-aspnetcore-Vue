import Vue from 'vue'
import axios from 'axios'
import router from './router'
import store from './store'
import { sync } from 'vuex-router-sync'
import App from 'components/app-root'

import '@progress/kendo-ui'
import '@progress/kendo-theme-default/dist/all.css'

import { Grid, GridInstaller } from '@progress/kendo-grid-vue-wrapper'
import { DataSource, DataSourceInstaller } from '@progress/kendo-datasource-vue-wrapper'
import {
    AutoComplete,
    ComboBox,
    DropDownList,
    MultiSelect,
    DropdownsInstaller
} from '@progress/kendo-dropdowns-vue-wrapper'
import { Calendar, DateinputsInstaller } from '@progress/kendo-dateinputs-vue-wrapper'
import {
    Button,
    ButtonGroup,
    ButtonGroupButton,
    ToolBar,
    ToolBarItem,
    ButtonsInstaller
} from '@progress/kendo-buttons-vue-wrapper'

Vue.use(GridInstaller)
Vue.use(DataSourceInstaller)
Vue.use(DropdownsInstaller)
Vue.use(DateinputsInstaller);
Vue.use(ButtonsInstaller)

Vue.prototype.$http = axios;

sync(store, router)

const app = new Vue({
    store,
    router,
    components: {
        Grid,
        AutoComplete,
        ComboBox,
        DropDownList,
        MultiSelect,
        Button,
        ButtonGroup,
        ButtonGroupButton,
        ToolBar,
        ToolBarItem
    },
    ...App
})

export {
    app,
    router,
    store
}
