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
import fkMath from '@/utils/fkMath';
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
      sites: [],
    };
  },
  mounted() {
    this.svgWidth = this.$refs.vorMap.clientWidth;
    this.svgHeight = this.$refs.vorMap.clientHeight;

    const w = this.mapData.width;
    const dw = this.svgWidth / w;
    const halfDw = dw / 2;
    const h = this.mapData.height;
    const dh = this.svgHeight / h;
    const halfDh = dh / 2;

    this.sites = [];
    for (let i = 0; i < h; i++) {
      for (let j = 0; j < w; j++) {
        this.sites.push({
          x: fkMath.round(halfDw + (i * dw)),
          y: fkMath.round(halfDh + (j * dh)),
          row: i + 1,
          col: j + 1,
        });
      }
    }
  },
};
</script>

<style>
div.vor-map {
  width: 100%;
  height: 100%;
}
</style>
