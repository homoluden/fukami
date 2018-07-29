<template>
  <div
    ref="vorMap"
    class="vor-map" >
    <svg-canvas
      v-if="svgWidth && svgHeight"
      :width="svgWidth"
      :height="svgHeight">
      <svg-path
        v-for="cell in cells"
        :key="cell.key"
        :points="cell.points"/>
    </svg-canvas>
  </div>
</template>

<script>
import * as d3 from 'd3';
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
      cells: [],
    };
  },
  mounted() {
    this.svgWidth = this.$refs.vorMap.clientWidth;
    this.svgHeight = this.$refs.vorMap.clientHeight;

    this.generateSites();
    this.cells = this.generateCells();
  },
  methods: {
    generateSites() {
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
    generateCells() {
      const voronoi = d3.voronoi().extent([[-1, -1], [this.svgWidth + 1, this.svgHeight + 1]]);
      const diagram = voronoi(this.sites.map(s => [s.x, s.y]));
      let polygons = diagram.polygons();

      polygons = polygons.map((p) => {
        const result = {
          key: `s[${p.data.join(',')}]`,
          site: p.data,
          points: fkMath.roundArray(p),
        };

        return result;
      });

      return polygons;
    },
  },
};
</script>

<style>
div.vor-map {
  width: 100%;
  height: 100%;
}
</style>
