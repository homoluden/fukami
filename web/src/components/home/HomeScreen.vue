<template>
  <div class="canvas-wrapper">
    <svg-canvas
      width="1500"
      height="800">
      <svg-path
        data="M-1,144.37733L218.987952,255.9603L297.0178439,190.7354573L270.462359,-1L-1,-1Z"/>
    </svg-canvas>
  </div>
</template>

<script>
import SvgCanvas from '@/components/shared/svgCanvas';
import SvgPath from '@/components/shared/svgPath';

export default {
  name: 'HomeScreen',
  components: {
    SvgCanvas,
    SvgPath,
  },
};
</script>

<style>
div.canvas-wrapper {
  display: flex;
  flex-direction: column;
  justify-content: center;
  height: 100%;
}
</style>
