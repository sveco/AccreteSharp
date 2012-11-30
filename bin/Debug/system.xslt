<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:decimal-format name="dec" decimal-separator="," grouping-separator="." zero-digit="0"/>
  <xsl:template match="/">
    <html>
      <head>
        <Title>Systems</Title>
        <link rel="stylesheet" type="text/css" href="system.css" media="screen" />
      </head>
    </html>
    <body>
      <xsl:for-each select="Stars/StarSystem">
        <table class="tblStar">
          <tr>
            <td>Type:</td><td><xsl:value-of select="primary/CODE"/></td>
          </tr>
          <tr>
            <td>Mass:</td><td><xsl:value-of select="format-number(primary/SM, '0,0000', 'dec')"/></td>
          </tr>
          <tr>
            <td>Age:</td><td><xsl:value-of select="format-number(primary/age, '0,0000', 'dec')"/></td>
          </tr>
        </table>
        <table class="tblPlanet">
          <tr>
            <td>Orb. dist. (AU)</td>
            <td>Eccent.</td>
            <td>Density</td>
            <td>Mass (Earth = 1)</td>
            <td>Resonant</td>
            <td>Description</td>
            <td>Surface temp. (K)</td>
            <td>Surf. pressure</td>
            <td>Day lenght (hours)</td>
            <td>Year lenght (days)</td>
            <td>Surface G</td>
            <td>Moons</td>
            <td>Greenhouse</td>
          </tr>
          <xsl:for-each select="Planet">
            <tr>
              <td><xsl:value-of select="format-number(a, '0,0000', 'dec')"/></td>
              <td><xsl:value-of select="format-number(e, '0,0000', 'dec')"/></td>
              <td><xsl:value-of select="format-number(density, '0,0000', 'dec')"/></td>
              <td><xsl:value-of select="format-number(mass, '0,0000', 'dec')"/></td>
              <td>
                <xsl:if test="resonant_period=1">Yes</xsl:if>
                <xsl:if test="resonant_period=0">No</xsl:if>
              </td>
              <td><xsl:value-of select="description"/></td>
              <td>
                <xsl:choose>
                  <xsl:when test="surf_temp='9.9999E+37'">
                    N/A
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="format-number(surf_temp, '0,0000', 'dec')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </td>
              <td>
                <xsl:choose>
                  <xsl:when test="surf_pressure='9.9999E+37'">
                    N/A
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="format-number(surf_pressure, '#.###,0000', 'dec')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </td>
              <td><xsl:value-of select="format-number(day, '#.###,0000', 'dec')"/></td>
              <td><xsl:value-of select="format-number(orb_period, '#.###,0000', 'dec')"/></td>
              <td>
              <xsl:choose>
                <xsl:when test="surf_grav='9.9999E+37'">
                  N/A
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="format-number(surf_grav, '#.###,0000', 'dec')"/>
                </xsl:otherwise>
              </xsl:choose>
              </td>
              <td><xsl:value-of select="moons"/></td>
              <td><xsl:value-of select="greenhouse_effect"/></td>
              <!--<td><xsl:value-of select="atmosphere"/></td>-->
            </tr>
          </xsl:for-each>
        </table>      
      </xsl:for-each>
    </body>
  </xsl:template>
</xsl:stylesheet>