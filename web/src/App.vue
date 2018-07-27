<template>
  <div id="app">
    <div class="page-container md-layout-column">
      <md-toolbar class="md-primary md-layout md-alignment-center-space-between">
        <div class="toolbar-container left">
          <md-button class="md-icon-button" @click="showNavigation = true">
            <md-icon>menu</md-icon>
          </md-button>
        </div>
        <span class="md-title md-alignment-center">Fukami Game</span>
        <div class="toolbar-container right"/>
      </md-toolbar>

      <md-drawer :md-active.sync="showNavigation">
        <md-toolbar class="md-transparent" md-elevation="0">
          <span class="md-title">Menu</span>
        </md-toolbar>

        <md-list>
          <md-list-item>
            <md-icon>library_books</md-icon>
            <span class="md-list-item-text">Readings</span>
          </md-list-item>

          <md-list-item>
            <md-icon>videogame_asset</md-icon>
            <span class="md-list-item-text">Demos</span>
          </md-list-item>

          <md-list-item>
            <md-icon>help</md-icon>
            <span class="md-list-item-text">About</span>
          </md-list-item>

        </md-list>
      </md-drawer>

      <md-content class="main-content">
        <router-view/>
      </md-content>
    </div>
  </div>
</template>

<script>
export default {
  name: 'app',
  data() {
    return {
      showNavigation: false,
    };
  },
};
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

div.toolbar-container {
  width: 100px;
}
</style>

<style lang="scss">
@import "~vue-material/dist/theme/engine";

$active-border-color: md-get-palette-color(black, 300);
.page-container {
  min-height: 100vh;
  overflow: hidden;
  position: relative;
  border: 1px solid rgba(#000, .12);
}

.md-drawer {
  width: 230px;
  max-width: calc(100vw - 125px);
}

.md-content {
  padding: 16px;
}

.main-content {
  position: fixed;
  top: 64px;
  bottom: 0;
  left: 0;
  right: 0;
  padding: 0;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: flex-start;
  overflow: hidden;
  border: 1px solid rgba(#000, .26);
  background: rgba(#000, .06);
}
</style>
