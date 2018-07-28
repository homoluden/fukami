import Vue from 'vue';
import HomeScreen from '@/components/home/HomeScreen';

describe('HomeScreen.vue', () => {
  it('should render correct contents', () => {
    const Constructor = Vue.extend(HomeScreen);
    const vm = new Constructor().$mount();
    expect(vm.$el.querySelector('.hello h1').textContent)
    .toEqual('Welcome to Your Vue.js App');
  });
});
