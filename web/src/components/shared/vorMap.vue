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
        :points="cell.points"
        :active-cell="cell.site.activeCell"
        :preview-cell="onCellPreview"/>
    </svg-canvas>
  </div>
</template>

<script>
import * as d3 from 'd3';
import {
  debounce,
  isObject,
} from 'lodash';
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
  beforeDestroy() {
    window.removeEventListener('resize', this.resizeHandler);
  },
  mounted() {
    window.addEventListener('resize', this.resizeHandler = debounce(this.regenerateCells, 300));
    this.regenerateCells();
  },
  methods: {
    regenerateCells() {
      this.svgWidth = this.$refs.vorMap.clientWidth;
      this.svgHeight = this.$refs.vorMap.clientHeight;

      this.generateSites();
      this.cells = this.generateCells();
    },
    generateSites() {
      const w = this.mapData.width;
      const dw = this.svgWidth / w;
      const halfDw = dw * 0.5;
      const qtrDw = dw * 0.35;
      const h = this.mapData.height;
      const dh = this.svgHeight / h;
      const halfDh = dh * 0.5;
      const qtrDh = dh * 0.35;

      this.sites = [];
      for (let i = 0; i < h; i++) {
        for (let j = 0; j < w; j++) {
          let x = (j * dw) + halfDw;
          x += (Math.random() * halfDw) - qtrDw;
          let y = (i * dh) + halfDh;
          y += (Math.random() * halfDh) - qtrDh;

          const s = {
            x: fkMath.round(x),
            y: fkMath.round(y),
            row: i + 1,
            col: j + 1,
          };

          const activeCell = this.mapData.activeCells
            .find(ac => ac.row === s.row && ac.col === s.col);
          if (activeCell) {
            s.activeCell = activeCell;
          }

          this.sites.push(s);
        }
      }
    },
    generateCells() {
      const voronoi = d3.voronoi().extent([[-1, -1], [this.svgWidth + 1, this.svgHeight + 1]]);
      const diagram = voronoi(this.sites.map(s => [s.x, s.y]));
      let polygons = diagram.polygons();

      polygons = polygons.map((p, i) => {
        const result = {
          key: `s[${p.data.join(',')}]`,
          site: this.sites[i],
          points: fkMath.roundArray(p),
        };

        return result;
      });

      return polygons;
    },
    onCellPreview(activeCell) {
      if (!isObject(activeCell)) {
        return;
      }

      this.$swal('Active Cell clicked');
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
