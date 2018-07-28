<template>
  <div class="canvas-wrapper">
    <vor-map
      v-if="mapData"
      :map-data="mapData"/>
  </div>
</template>

<script>
import vorMap from '@/components/shared/vorMap';
import homeMap from '@/data/maps/home.json';

export default {
  name: 'HomeScreen',
  components: {
    vorMap,
  },
  data() {
    return {
      mapData: homeMap,
    };
  },
};
</script>

<style>
div.canvas-wrapper {
  display: flex;
  flex-direction: column;
  justify-content: center;
  height: 100%;
  width: 100%;
}
</style>
