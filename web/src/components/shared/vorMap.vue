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
    this.svgWidth = this.$refs.vorMap.clientWidth;
    this.svgHeight = this.$refs.vorMap.clientHeight;
  },
};
</script>

<style>
div.vor-map {
  width: 100%;
  height: 100%;
}
</style>
