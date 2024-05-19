import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
//import store from './store';
import 'bootstrap'
import 'devextreme/dist/css/dx.light.css';

createApp(App).use(router).mount('#app')
