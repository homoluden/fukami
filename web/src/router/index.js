import Vue from 'vue';
import Router from 'vue-router';
import HomeScreen from '@/components/home/HomeScreen';
import DraftsScreen from '@/components/drafts/DraftsScreen';
import RawScreen from '@/components/raw/RawScreen';

Vue.use(Router);

export default new Router({
  routes: [
    {
      path: '/',
      name: 'HomeScreen',
      component: HomeScreen,
    },
    {
      path: '/drafts',
      name: 'DraftsScreen',
      component: DraftsScreen,
    },
    {
      path: '/raw',
      name: 'RawScreen',
      component: RawScreen,
    },
  ],
});
