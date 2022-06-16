<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.8" tiledversion="1.8.4" name="forest-wide" tilewidth="32" tileheight="32" tilecount="64" columns="8">
 <image source="images/gras-tiles-32px.png" width="256" height="256"/>
 <tile id="2">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="24" width="32" height="8"/>
  </objectgroup>
 </tile>
 <tile id="6">
  <properties>
   <property name="Wall" type="bool" value="true"/>
  </properties>
 </tile>
 <tile id="9">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0.0625">
    <polygon points="0,0 0,31.75 32,0.0625"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="10" probability="0.5"/>
 <tile id="11">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0.125" y="0.125">
    <polygon points="0,0 31.6875,31.6875 31.75,-0.1875"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="14">
  <properties>
   <property name="Wall" type="bool" value="true"/>
  </properties>
 </tile>
 <tile id="16">
  <objectgroup draworder="index" id="2">
   <object id="1" x="29" y="0" width="3" height="32"/>
  </objectgroup>
 </tile>
 <tile id="17" probability="0.5"/>
 <tile id="18" probability="5"/>
 <tile id="19" probability="0.5"/>
 <tile id="20">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="3" height="32"/>
  </objectgroup>
 </tile>
 <tile id="22" probability="10">
  <properties>
   <property name="Wall" type="bool" value="true"/>
  </properties>
 </tile>
 <tile id="25">
  <objectgroup draworder="index" id="2">
   <object id="1" x="-0.375" y="-0.0625">
    <polygon points="0,0 32.125,32.25 0.3125,32.125"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="26" probability="0.5"/>
 <tile id="27">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0.25" y="31.8125">
    <polygon points="0,0 31.5625,-31.5625 31.8125,0.375"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="34">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="32" height="31"/>
  </objectgroup>
 </tile>
 <tile id="40">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="31">
    <polygon points="0,0 26,-3 31,-31 0,-31"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="41">
  <objectgroup draworder="index" id="3">
   <object id="2" x="1" y="0">
    <polygon points="0,0 5,27 31,31 31,0"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="48">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="1">
    <polygon points="0,0 27,3 31,31 0,31"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="49">
  <objectgroup draworder="index" id="2">
   <object id="1" x="1" y="32">
    <polygon points="0,0 3,-27 31,-31 31,0"/>
   </object>
  </objectgroup>
 </tile>
</tileset>
