<template>
  <div id="app">
    <div class="page-container md-layout-column">
      <md-toolbar class="md-primary md-layout md-alignment-center-space-between">
        <md-button class="md-icon-button" @click="showNavigation = true">
          <md-icon>menu</md-icon>
        </md-button>
        <span class="md-title md-alignment-center">Fukami Game</span>
        <md-button @click="showSidepanel = true"><md-icon  class="md-size-2x">face</md-icon></md-button>
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

      <md-drawer class="md-right" :md-active.sync="showSidepanel">
        <md-toolbar class="md-transparent" md-elevation="0">
          <span class="md-title">User Info</span>
        </md-toolbar>

        <md-list>
          <md-list-item>
            <span class="md-list-item-text">John Smith</span>

            <md-button class="md-icon-button md-list-action">
              <md-icon class="md-primary">email</md-icon>
            </md-button>
          </md-list-item>

        </md-list>
      </md-drawer>

      <md-content class="main-content">
        <router-view/>
      </md-content>
      <md-bottom-bar class="md-alignment-top-center" md-type="shift" :md-theme="'bottom-bar-' + theme" md-sync-route="true">
        <md-bottom-bar-item to="/" md-label="Ready" md-icon="done_all" @click="theme = 'teal'"></md-bottom-bar-item>
        <md-bottom-bar-item to="/drafts" md-label="Drafts" md-icon="rounded_corner" @click="theme = 'orange'"></md-bottom-bar-item>
        <md-bottom-bar-item to="/raw" md-label="Raw" md-icon="block" @click="theme = 'red'"></md-bottom-bar-item>
      </md-bottom-bar>
    </div>
  </div>
</template>

<script>
export default {
  name: 'app',
  data() {
    return {
      theme: 'teal',
      showNavigation: false,
      showSidepanel: false,
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

  // Demo purposes only
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
  bottom: 57px;
  left: 0;
  right: 0;
  padding: 0;
  display: flex;
  align-items: center;
  justify-content: flex-start;
  overflow: hidden;
  border: 1px solid rgba(#000, .26);
  background: rgba(#000, .06);
}

.md-bottom-bar {
  position: fixed;
  left: 0;
  right: 0;
  bottom: 0;
}

.md-bottom-bar .md-bottom-bar-item.md-active {
  width: initial;
  border-top: 5px $active-border-color solid;
}

.md-bottom-bar>.md-ripple {
  justify-content: center;
}
</style>

<style lang="scss">
  @import "~vue-material/dist/theme/engine";

  @include md-register-theme("bottom-bar-teal", (
    primary: md-get-palette-color(teal, A200)
  ));

  @include md-register-theme("bottom-bar-orange", (
    primary: md-get-palette-color(orange, A200)
  ));

  @include md-register-theme("bottom-bar-blue", (
    primary: md-get-palette-color(blue, A200),
    accent: md-get-palette-color(red, A200)
  ));

  @include md-register-theme("bottom-bar-red", (
    primary: md-get-palette-color(red, A200)
  ));

  @include md-register-theme("bottom-bar-black", (
    primary: md-get-palette-color(lightblue, 900)
  ));

  @import "~vue-material/dist/theme/all";
</style>
