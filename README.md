<div align="left">
  
  <h1>WorkingTitle-SurvivorDefense</h1>

  <p>
    This game was developed as a student project in 10 weeks at the <a href="https://www.school4games.net">School For Games</a> in Berlin! <br />
    <b>IMPORTANT!</b> This repo represents an earlier stage of developement of the game <a href="https://suchti0352.itch.io/scrap-fever">Scrap Fever</a> (<a href="https://github.com/BasKrueger/ScrapFever/tree/main">Repo</a>)<br />
    I left the developement of the game, but only because another team lost their UI programmer.<br />
    Meanwhile <b><i>this</i></b> project was about to change it's scope to something much smaller for unrelated reasons.<br />
    A lot of unfinished features are still in this version of the game.
    <br /> <br />
  </p>

https://github.com/LumiToad/WorkingTitle-SurvivorDefense/assets/129980440/9b6f76e6-2f16-4df0-8ef3-aa84d3239053

  <hr />
  <h2>Engines / Languages</h2>
  Unity 2022.3.5f1, C#, Protobuf proto3

  <hr />
  <h2>Roles up to this point of developement</h2>
  - Gameplay programming -<br />
  - "Various things" programmer (Saving game / Options) -<br />
  - Communication with team -
  
  <hr />
  <h2>Responsiblities</h2>
  - Some gameplay programming -<br /><br />
  Some examples:
  <ul>
    <li><a href="https://github.com/LumiToad/WorkingTitle-SurvivorDefense/tree/main/Assets/Scripts/Camera">Camera scrolling for level sections (cut in final game)<a/></li>
      This one was a challenge, for several reasons.<br />
      I learned about the importance of communicating with your lead engineer.<br />
      Also the entire feature would have been much easier to realise without using the Cinemachine plugin<br />
    <li><a href="https://github.com/LumiToad/WorkingTitle-SurvivorDefense/blob/main/Assets/Scripts/Player/Player.cs">Out of Bounds respawn<a/></li>
      The player script was mostly programmed by [BasKrueger](https://github.com/BasKrueger).<br />
      Methods for the respawn are: DamageOnOutsideOfScreen(), GetSaveTeleportSpot(...), TeleportToCenter().<br />
      It will check in a circle from the middle of the screen,<br /> whether or not the player can spawn there, if you get pushed off screen.<br />
      <img src="https://github.com/LumiToad/WorkingTitle-SurvivorDefense/assets/129980440/68d75985-f398-459f-8796-98e5493395b4" width="450" height="250" />

  </ul>
  - Various programming -<br /><br />
  <ul>
    <li><a href="https://github.com/LumiToad/WorkingTitle-SurvivorDefense/blob/main/Assets/Scripts/Translation/CSVLanguageFileParser.cs">CSV parser for language files<a/></li>
    <li><a href="https://github.com/LumiToad/WorkingTitle-SurvivorDefense/blob/main/Assets/Scripts/Translation/TextByLanguage.cs">A system which will replace texts in TestMeshPro<a/></li>
    <li><a href="https://github.com/LumiToad/WorkingTitle-SurvivorDefense/blob/main/Assets/Scripts/Settings/SaveFileUtils.cs">Saving files<a/></li>
      Only a fraction of this stuff was used later on. Again, communication was an issue.
    <li><a href="https://github.com/LumiToad/WorkingTitle-SurvivorDefense/tree/main/Assets/Scripts/Settings">Settings / Options<a/></li>
    <li><a href="https://github.com/LumiToad/WorkingTitle-SurvivorDefense/blob/main/Assets/Scripts/Game/UI/FadeScreen.cs">Screen Fade in / out<a/></li>
  </ul>
    <br />
  - Communication with team -<br /><br />
  Should be a given.<br />
  This is specifically about technincal communication, such as explaining and helping with version control,<br />
  technical workflows for the implementation of 2D / 3D assets in Unity and so on.<br />
  For example, we had to figure out some 3D modeling guidelines together, because the high amount of<br />
  enemies were a performance challenge.
  <hr />
  <h2>Downloads and Website</h2>

  <h3>Itch</h3>
  <a href="https://suchti0352.itch.io/scrap-fever">
    <img src="https://github.com/LumiToad/LumiToad/blob/main/img/banner/github_scrap_banner.png" alt="scrap banner" />
  </a>
  
</div>
