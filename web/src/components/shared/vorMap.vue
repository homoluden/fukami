<template>
  <div
    ref="vorMap"
    class="vor-map" >
    <svg-canvas
      v-if="svgWidth && svgHeight"
      :width="svgWidth"
      :height="svgHeight">
      <svg-path />
    </svg-canvas>
  </div>
</template>

<script>
import SvgCanvas from './svgCanvas';
import SvgPath from './svgPath';

export default {
  components: {
    SvgCanvas,
    SvgPath,
  },
  props: {
    mapData: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      svgWidth: 0,
      svgHeight: 0,
    };
  },
  mounted() {
    this.data.svgWidth = this.$refs.vorMap.width;
    this.data.svgHeight = this.$refs.vorMap.height;
  },
};
</script>

<style>
</style>
