using System;
using Business.Common.Extensions;
using Business.HttpClient.Navigation;
using Flurl;
using Flurl.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using Business.Common.Configuration;
using Newtonsoft.Json.Linq;
using WebTrakrData.Services;

namespace BasicTests.HttpClient
{
    [TestClass]
    public class FullContactPersonServiceTest
    {
        #region Class Initialization

        public FullContactPersonServiceTest()
        {
            configMgr = new ConfigMgr();
            service = new FullContactPersonService(configMgr.GetAppSetting("fc:X-FullContact-APIKey"));
        }

        private ConfigMgr configMgr { get; set; }
        private FullContactPersonService service { get; set; }

        #endregion

        [TestMethod]
        public void TestFlurlGetPersonByEmail_SUCCESS()
        {
            var email = @"dustin.lawler@datadoghq.com";

            JObject result;
            if (service.TryGetPersonByEmailJObject(email, out result))
            {
                // success
                Assert.IsNotNull(result);
            }
            else
            {
                // fail
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void TestFlurlGetPersonByEmail_FAIL()
        {
            var email = @"vtabacco@castcrete.com";

            JObject result;
            if (service.TryGetPersonByEmailJObject(email, out result))
            {
                // success
                Assert.IsNotNull(result);
            }
            else
            {
                // fail
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void TestFlurlGetPersonsByEmail()
        {
            return;
            // Temporarily disabled

            var successArr = new JArray();
            var failArr = new JArray();
            var emails = getEmailList();

            foreach (var email in emails)
            {
                System.Threading.Thread.Sleep(2000);
                try
                {
                    JObject result;
                    if (service.TryGetPersonByEmailJObject(email, out result))
                    {
                        successArr.Add(result);
                    }
                    else
                    {
                        failArr.Add(result);
                    }
                }
                catch (Exception ex)
                {
                    //throw;
                }
            }
            try
            {
                successArr.WriteJsonToFile(new FileInfo(@"C:\junk\successFullContactsTest.json"));
            }
            catch (Exception ex)
            {
                //throw;
            }
            try
            {
                failArr.WriteJsonToFile(new FileInfo(@"C:\junk\failedFullContactsTest.json"));
            }
            catch (Exception ex)
            {
                //throw;
            }
        }






        [TestMethod]
        public void TestFlurlGetPersonByTwitter_SUCCESS()
        {
            var twitter = @"tcg_carl";

            JObject result;
            if (service.TryGetPersonByTwitterJObject(twitter, out result))
            {
                // success
                Assert.IsNotNull(result);
            }
            else
            {
                // fail
                Assert.IsNotNull(result);
            }
        }


        [TestMethod]
        public void TestFlurlGetPersonByPhone_FAIL()
        {
            var phone = @"3037170414";

            JObject result;
            if (service.TryGetPersonByTwitterJObject(phone, out result))
            {
                // success
                Assert.IsNotNull(result);
            }
            else
            {
                // fail
                Assert.IsNotNull(result);
            }
        }

        private List<string> getEmailList()
        {
            return new List<string>
            {
                //"yacj@novozymes.com",
                //"dustin.lawler@datadoghq.com",
                //"carl@companion-group.com",
                //"rina.leonard@hs.utc.com",
                //"wwarren@howardmccray.com",
                //"terryh@gpatpa.com",
                //"steig@tessera.com",
                //"flewallen@okoha.com",
                //"rrudolph@westsidebeer.com",
                //"ssatterthwaite@homier.com",
                //"rong@arjinfusion.com",
                //"pierce@sparkbase.com",
                //"ajones@fool.com",
                //"fhildenbrand@bierycheese.com",
                //"tmiller@esncc.com",
                //"heidi.oddvik@nordisk-aviation.com",
                //"ldacosta@tri-intl.com",
                //"mtiberio@convexitycapital.com",
                //"nkhan@nextfinancial.com",
                //"sgoodwin@danielshealth.com",
                //"timur@keyelco.com",
                //"mwinemiller@gosiger.com",
                //"eaviles@c-path.org",
                //"greg.kirsch@ansay.com",
                //"mrozekm@karmanos.org",
                //"rmccurdy@noviacareclinics.com",
                //"mkarasek@paynecrest.com",
                //"pkant@mtc.ca.gov",
                //"cparks@healthplus.com",
                //"rfrias@gpl4u.com",
                //"rlehmann@cliffordpaper.com",
                //"mattingly@onlifehealth.com",
                //"bradley.frohman@unos.org",
                //"awilliams@americanfamilycare.com",
                //"matthew.frankenfield@trifecta.com",
                //"dmcwhorter@intellidyne-llc.com",
                //"john.gaines@wwbic.com",
                //"jhawks@gevatheatre.org",
                //"jacob.mathews@tdsecurities.com",
                //"mmitchell@capitoldist.com",
                //"sheldon.hinkson@casedata.com",
                //"bcovell@msdcapital.com",
                //"mcannizzaro@pryorcashman.com",
                //"kenny@sbesllc.com",
                //"cmayes@midsouthrehab.com",
                //"enriquez@phillipsind.com",
                //"kmaynard@gavelintl.com",
                //"shollowell@smithbrosfurn.com",
                //"jim.house@stl.unitedway.org",
                //"dsteeley@ljbinc.com",
                //"pnicholls@lgamerica.com",
                //"daryl@abtechtechnologies.com",
                //"roman.gelfand@plasticexpress.com",
                //"jchott@cobank.com",
                //"rodenthel@thepeaks.org",
                //"pcoiner@gpssource.com",
                //"rcastaneda@centaurusfinancial.com",
                //"lallen@collier.org",
                //"eric.steen@proof-advertising.com",
                //"dminton@cphcorp.com",
                "vtabacco@castcrete.com",
                "jerryw@dutchmaid.com",
                "michelle.peairs@mckesson.com",
                "shyam@combine.com",
                "emil.lavagno@mambro.com",
                "mstonebraker@paradigm4.com",
                "marty@source1cable.com",
                "mlorge@willmanind.com",
                "cberglin@seafieldcenter.com",
                "lpeel@horizonind.com",
                "dlou@pollinhotels.com",
                "brad.light@sybase.com",
                "jody.kohl@communitycarecw.org",
                "mike@grove-temporary.com",
                "florena.masa@nissinfoods.com",
                "pespinoza@daisybrand.com",
                "ralph.perrella@dealogic.com",
                "cpetrus@orbitaltool.com",
                "tchichester@altaits.com",
                "ronda.freeman@parivedasolutions.com",
                "creynolds@hospices.org",
                "gtacuri@its.jnj.com",
                "dhom@mocins.com",
                "aaugustine@smarsh.com",
                "sfeda@hubbardhall.com",
                "rob.mcelhaney@nashville.gov",
                "kenneth.polky@gibault.org",
                "juliec@westwoodcountryclub.com",
                "sonu@seasons.org",
                "tsacrey@paksher.com",
                "gnaden@geoforce.com",
                "mjmorey@lrca.com",
                "chris.heilig@evergegroup.com",
                "tjimenez@fortistar.com",
                "jamal.pilger@onyxmd.com",
                "jwhite@hunzicker.com",
                "eric.powell@freudenberg-it.com",
                "jpope@stalexiushospital.com",
                "kevin.hertz@telcentris.com",
                "maherrera@oxfamamerica.org",
                "kcleverly@shipleywins.com",
                "sgreer@softub.com",
                "dlafrankie@greenacrescontracting.com",
                "adam@infoprogroup.com",
                "brett.stewart@acumera.net",
                "pjones@aqha.org",
                "eugene.oh@sgtransit.com",
                "jhoward@supportunitedway.org",
                "chris.johnson@scra.org",
                "briano.ruiz@solidexecutive.com",
                "jpowell@ipipes.com",
                "bob.ulrich@goamp.com",
                "richard.kenneally@rbccm.com",
                "tvaclavicek@genesissolutions.com",
                "sushrut@cal2cal.com",
                "tnugent@wilsbach.com",
                "amy.ramirez@apsva.us",
                "jcrysdale@napower.com",
                "nferguson@peregrineinc.com",
                "sheldon@westernstatesflooring.com",
                "kcraycraft@executivebank.com",
                "jorge.henriquez@horizontechnology.com",
                "sam.gaines@balfour.com",
                "jtimlin@swanderson.com",
                "ksavitz@hamiltoninsurance.com",
                "jmiller@inclusioninc.org",
                "cwilson@hill-wilkinson.com",
                "beata.fred@rgsinc.com",
                "charlie@itworks-software.com",
                "david@sjdenham.com",
                "dsherrin@atgfreight.com",
                "eddie.krebs@metrobrokers.com",
                "bkoehn@milwaukeebroach.com",
                "tjordan@hendrickhealth.org",
                "annetteo@anderson-electric.com",
                "randy.hendrickson@us.kspg.com",
                "pat.lucas@carecamhealthsystems.com",
                "pdyk@sunlife.com",
                "mgrubbs@raycountyhospital.com",
                "dkosinski@whdlaw.com",
                "jrankin@douglascherokee.org",
                "pat.walsh@protocallservices.com",
                "tpickrell@cincom.com",
                "howard.weiss@parsons.com",
                "slemler@childfund.org",
                "jt@lynbrookglass.com",
                "curtis.goodwin@kihomac.com",
                "tom@straitsteel.com",
                "dshaffer@taggl.com",
                "ggadoua@lpciminelli.com",
                "tjain@enduranceservices.com",
                "jwinker@baycominc.com",
                "jlindsay@barlowtruckline.com",
                "reena.rodgers@equian.com",
                "adiazdelguante@specialtyequipment.com",
                "tschmalz@hwmuw.org",
                "adick@comnet-fids.com",
                "tom@thebarnesfirm.com",
                "csharp@certifit.com",
                "nate@hagangroup.com",
                "jukka.jarvenpaa@primapower.com",
                "devin.strunk@cellularsales.com",
                "ray@peeples.com",
                "rwerksman@edgarsnyder.com",
                "drobson@focuscamera.com",
                "jason.bechstein@distinctivestatuary.com",
                "banquets@glacierbrewhouse.com",
                "brandone@101-fm.com",
                "layton.johnson@paradigmprecision.com",
                "garyk@carmelfinancial.com",
                "toms@trostel.com",
                "ssrinivasan@telesphere.com",
                "rgaustad@earthsystems.com",
                "dthalacker@foundation.iastate.edu",
                "agiancola@millerautomotive.com",
                "rick.chappel@zilliant.com",
                "jbaker@mebs.com",
                "wendelburg@asgvets.com",
                "a.chari@tropos.com",
                "soma.pullela@thetradedesk.com",
                "jkim@humaxdigital.com",
                "ekippel@amazingfacts.org",
                "hhoward@aristainfo.com",
                "cboling@cpiequipment.com",
                "jingram@mueller-yurgae.com",
                "kmatthews@wolfsoncasing.com",
                "ebozzetti@excelled.com",
                "mark.carr@dakotacare.com",
                "bill.hoffman@kitware.com",
                "sharon.addison@carecentrix.com",
                "rick@brooksrand.com",
                "mrakic@xorantech.com",
                "asfand.saeed@tibercreek.com",
                "kraymond@learningresources.com",
                "ross@1stdibs.com",
                "jveritch@nationalenzyme.com",
                "pevans@joesstonecrab.com",
                "sue.clements@hhs.co.santa-clara.ca.us",
                "thil@callbpi.com",
                "mbmoser@pilotchemical.com",
                "lvarela@unitedelectric.com",
                "cynthiar@uic.edu",
                "tparisi@stonehengeny.com",
                "kbeijer@mortgagecadence.com",
                "flynn@strategicstaff.com",
                "fperin@healthintegrated.com",
                "kenz@serviceengineering.com",
                "paige.needling@recall.com",
                "gwise@heartlanddentalcare.com",
                "ipaez@rair.com",
                "bmullen@vanderweil.com",
                "marksalrin@coreconstruction.com",
                "stakhar@innotas.com",
                "hstout@premiermagnesia.com",
                "bmceuen@lombartinstrument.com",
                "dtownsend@sainc.com",
                "rhett@plaqbank.com",
                "mluo@premiumit.com",
                "lchilds@perkinscpas.com",
                "david.solovey@amwins.com",
                "dphillips@freightmgmt.com",
                "pkdogbatse@tmhs.org",
                "renee@mataninc.com",
                "dyoung@alz.org",
                "sean@adfoods.com",
                "jacob.mortimer@expressmedrx.com",
                "vmacedo@mafindustries.com",
                "cdifolco@callico.com",
                "chris@pioneerbanktexas.com",
                "mbrennan@devonhealth.com",
                "rturcotte@datx.com",
                "mmulnix@seattlemortgage.com",
                "phedlund@uphp.com",
                "gdefari@shellvacations.com",
                "vig@medhok.com",
                "nic.holmes@chempoint.com",
                "pcrum@umc.edu",
                "gcrowson@specrubber.com",
                "tomn@reeve-knight.com",
                "kmotaghed@moog.com",
                "kamin@fosdickandhilmer.com",
                "goldberg@nymcu.org",
                "lklein@ediets.com",
                "jpatterson@cornerstonebankfl.com",
                "tlamers@usinger.com",
                "jgoldberg@marketaxess.com",
                "silliman@ardeinc.com",
                "greg@opusing.com",
                "c.slingerland@enternest.com",
                "martin.thorby@retalix.com",
                "slastic@sunprintmanagement.com",
                "dtaylor@upcoinc.com",
                "israel.jim@irisusainc.com",
                "mark.cantelli@xerox.com",
                "edward.moh@normagroup.com",
                "anand@sgstechnologies.net",
                "jake@axiomfinancial.com",
                "jaime@wepow.com",
                "pstephan@viewpost.com",
                "jmckercher@umm.edu",
                "jharris@rrj.state.va.us",
                "jody.greene@mrm-mccann.com",
                "gshaffer@unitedwaytarrant.org",
                "james.lee@worldjournal.com",
                "irina.kruzhkova@octapharma.com",
                "dstory@wwfcu.org",
                "dkelly@roadrunnersports.com",
                "kherrington@nexsan.com",
                "tell@statoil.com",
                "nelson.tasia@controlledpwr.com",
                "davidb@hi-tempinsulation.com",
                "justin.shipp@pw.utc.com",
                "jmonroe@garrtool.com",
                "bmcallister@lehighgas.com",
                "aido@healthadvocate.com",
                "dgillon@suntechmed.com",
                "jmcintire@thelpsb.com",
                "astine@lepmed.com",
                "gfox@alfaaic.com",
                "cliff.ireland@floodandpeterson.com",
                "ndargenio@lfymcd.com",
                "dhaustveit@victoriaclipper.com",
                "howardw@vadrillco.com",
                "chrisr@tensilica.com",
                "cferreira@hydraquip.com",
                "renaud@eventbrite.com",
                "tjohnsto@wcsi.org",
                "jmillman@kasslaw.com",
                "manish.balsara@reputation.com",
                "bhendrix@dentonrc.com",
                "ahoway@mobilemedical.org",
                "john.bush@iberdrolaren.com",
                "georgegoodwin@centralite.com",
                "rpuertoreal@visiumfunds.com",
                "edgar@helvetia-pr.com",
                "rknorwood@maryhaven.com",
                "ravi.raj@dovetailhealth.com",
                "meberhardt@marotta.com",
                "frank@towncaredental.com",
                "jgeorge@centurionstone.com",
                "brado@automationtool.com",
                "paul.ward@fmc-ag.com",
                "mmargosian@egefcu.org",
                "nasirbilloo@mariettaga.gov",
                "staci@ctbonline.com",
                "markdoff@kaytee.com",
                "seanmc@comtechfay.com",
                "sean@skis.com",
                "george.romo@gilacorp.com",
                "briank@summitcu.org",
                "b.lain@kilopass.com",
                "ajith.gorijala@suezenergyna.com",
                "mnguyen@sis.us",
                "jeremy.miller@ais-york.com",
                "faustino.gomez@londen-insurance.com",
                "adecesaris@lionbrewery.com",
                "jgarner@tmecorp.com",
                "lkrill@alicoinc.com",
                "isidro.hernandez@aceglass.net",
                "dwhite@twperry.com",
                "greg@firesidetheatre.com",
                "sylvain.chambon@gemalto.com",
                "dschaefers@unionsupply.com",
                "dwhite@bnncpa.com",
                "kevin.wilkinson@gallusbiopharma.com",
                "dbarry@windsorcourthotel.com",
                "rromeo@buddlarner.com",
                "phil.greene@chartercon.com",
                "aanderson@q2ebanking.com",
                "rmroczkowski@haletrailer.com",
                "jason.nichols@avitecture.com",
                "byronp@andrew-williamson.com",
                "kkrull@elevate97.com",
                "h.federman@vacco.com",
                "aguerrero@jeinc.com",
                "lonnieb@psba.com",
                "gmisenar@wrightrunstad.com",
                "kehinde.s.akinfesoye@tsocorp.com",
                "augierivera@wcslending.com",
                "kedmunds@fsa.com",
                "mhopper@vanwagner.com",
                "toms@naylor.com",
                "rdemuth@ensequence.com",
                "michelle.martenis@hatterasyachts.com",
                "darrellb@ckautoparts.com",
                "rzeimet@universaldisplay.com",
                "dputt@onesourceh2o.com",
                "kwhipple@prldocs.com",
                "ramsey@rosenthallevy.com",
                "john_komer@progressive.com",
                "glonneman@lothinc.com",
                "arafat.ahmed@davison.com",
                "edz@hltool.com",
                "hsigler@millernash.com",
                "ram.gabriel@prulaney.com",
                "maryfran_johnson@cio.com",
                "jose.munoz@dyn-intl.com",
                "zzumot@forescout.com",
                "eap@propelinsurance.com",
                "rswart@cmdi.net",
                "kwade@eucalyptus.com",
                "vschoenfelder@captechventures.com",
                "miker@radarinc.com",
                "bmelby@thermomass.com",
                "aluch@izotope.com",
                "robert.kell@pw.utc.com",
                "mmarian@ecsconsult.com",
                "shaynes@blendtec.com",
                "azielinski@bwwlaw.com",
                "alo@meetsoci.com",
                "dhurley@besam-usa.com",
                "shane.rowe@altiercu.org",
                "fmorelli@computechinc.com",
                "kziegelbauer@glattair.com",
                "tom.cahill@wecu.com",
                "dave.wittwer@galtronics.com",
                "sbarnett@icg.aero",
                "greg.kalantzes@investools.com",
                "joseph.koenig@ptgcorp.com",
                "ncallahan@metprop.com",
                "mmarotti@bluestatedigital.com",
                "afattah@propergroupintl.com",
                "mgaugler@woodlandsbank.com",
                "k.hay@lumasenseinc.com",
                "aspahan@solomonpage.com",
                "jsanford@ubnt.com",
                "kevin.martin@psm.com",
                "spatterson@ddctvm.com",
                "raj.subramanian@bhs.org",
                "ndelaney@johnsonwilson.com",
                "lpayne@isa-net.com",
                "g.dew@fugro.com",
                "juliewoods@regencycenters.com",
                "lynn.armstrong@daarengineering.com",
                "egrober@pdmi.com",
                "anne.benvenuto@rdrassociates.org",
                "mpratt@commonwealth.com",
                "ggallant@sac.shiseido.com",
                "trobinson@bayareaturningpoint.com",
                "adkins.jeremy@lycos-inc.com",
                "glamica@cubbison.com",
                "t_duffy@luhr.com",
                "wsmith@netcareaccess.org",
                "vlentino@wpcarey.com",
                "llewis@renkim.com",
                "carl.burkholder@firstdata.com",
                "jimq@gorb.net",
                "sheryl@goodfoods.coop",
                "gfeigh@firstib.com",
                "rob@metritek.com",
                "weady@wbcarrellclinic.com",
                "blake.fish@asig.com",
                "steven.beaudrot@fmcna.com",
                "btaylor@aerometric.com",
                "blindsey@golfgalaxy.com",
                "m.minnich@netrada.com",
                "williamc@bittitan.com",
                "lpatterson@cckautomations.com",
                "wschneider@antillean.com",
                "gouldm@jwterrill.com",
                "matthew.cotterman@beldon.com",
                "mike.gabree@fidelity.com",
                "joe.mallinger@metalera.com",
                "blafary@baldwinandlyons.com",
                "mpawlus@elbex-us.com",
                "hwaters@ctsi-usa.com",
                "sarah@walshtrucking.com",
                "mlamoreaux@chuys.com",
                "jbutrovich@sparkunlimited.com",
                "shenkevansd@capitalareafoodbank.org",
                "eric.edmond@bankwithmutual.com",
                "ldorgan@mom365.com",
                "twenhold@powerhrg.com",
                "jsanchez@miox.com",
                "eddie.luz@steelsummit.com",
                "greg@carmelracquetclub.com",
                "mlloyd@thecitizensbank.com",
                "gary.mattox@honeywell.com",
                "lmuras@texaswilson.com",
                "gloveless@multimedicalsystems.com",
                "lkoulet@dataxu.com",
                "dennism@continuant.com",
                "ellens@firemtn.com",
                "adam.paul@thing5.com",
                "mayberry@mayberryelectric.com",
                "jcole@picernemh.com",
                "milica.popovic@cdmmedia.com",
                "frank.rios@gnax.net",
                "gvolochinsky@mdlab.com",
                "darrelltingle@cleancontrol.com",
                "etharakan@dsg-us.com",
                "rick@americanepay.com",
                "martina.winkler@grammer.com",
                "njoshi@thesba.com",
                "brculbertson@slandchc.com",
                "eleach@fhlbatl.com",
                "mowens@cullprop.com",
                "young@sas70solutions.com",
                "mwiltshire@capehart.com",
                "diane.carr@pdoor.com",
                "melissahahn@greatlakeslandscaping.com",
                "lgrisim@stanfordchildrens.org",
                "reed.mcconnell@sap.com",
                "rick.gerhardt@meierhoffer.com",
                "uschoop@globalsolar.com",
                "miless@darkhorse.com",
                "kevin.johnson@homedics.com",
                "sfitzsimmons@barnabashealth.org",
                "lhinkley@hampmach.com",
                "john.stevens@coleengineering.com",
                "stillinger@affinitysolutions.com",
                "daniel.dsaffd@unitedspacealliance.com",
                "thamilton@cellularsouth.com",
                "sgarcia@srds.com",
                "tom.ko@vitals.com",
                "mjackson@lewisadvertising.com",
                "kmurphy@cantorcolburn.com",
                "mjohnson@nefinc.org",
                "myoung@jackingram.com",
                "bbrown@spsmemphis.com",
                "jeng@renmac.com",
                "joe.mcmorris@datacert.com",
                "robert.inchausti@cargotec.com",
                "darrenb@vtbear.com",
                "eileenwalsh@apta.org",
                "plee@ilevelsolutions.com",
                "adamashley@medgestore.com",
                "elaine.mars@ppgulfcoast.org",
                "dmike@atlinks.com",
                "jthielen@pharmaca.com",
                "nathan@merchantoverstock.com",
                "rnorman@sourcelink.com",
                "david.erickson@rieter.com",
                "sfr@pvbrick.com",
                "dennis.sheperd@hrl.com",
                "kirsten.krsulj@essentialpowerllc.com",
                "norman@salesmasterflooring.com",
                "drew.weiss@refinery29.com",
                "brianh@riversidetransport.com",
                "jkim@datagram.com"
            };
        }
    }
}
