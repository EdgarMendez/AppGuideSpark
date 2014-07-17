<%@Page Debug="true" Title="Application Guide" Language="VB" MasterPageFile="~/MSDMain.master" AutoEventWireup="false" CodeFile="AppGuideDomestic.aspx.vb" Inherits="AppGuideDomestic" %>

<asp:Content ID="cntAppGuideContent" ContentPlaceHolderID="ContentPlaceHolderMSD" Runat="Server">
    <form id="FormMSD" runat="server">
    <div id="appGuide"></div>
	   <div style="margin-left:20px;">
	   
	   <p>Find MSD Ignition products for a specific year, make, model, and engine size.<br />
       
       <img src="images/PonyCar.gif" alt="MSD your Pony Car!" width="230" height="230" border="0" align="right" usemap="#Map3">
<map name="Map3">
  <area shape="rect" coords="19,99,212,136" href="/MustangLP.aspx" alt="Ford">
  <area shape="rect" coords="19,142,212,172" href="/CamaroLP.aspx" alt="Chevy">
  <area shape="rect" coords="19,183,212,216" href="/MoparLP.aspx" alt="Chrysler">
</map>
        
        
	   <p>Please note that not all vehicles are listed. If you cannot find your vehicle, please call our Tech Line at 915-855-7123.</p>
        <table>
					
			<tr>
                <td><asp:Label Visible="true" ID="lblYear" runat="server" text="Year:" /></td>
                <td>
                    <asp:DropDownList   ID="ddYear" runat="server" AutoPostBack="True" 
                                        DataSourceID="YearDS" DataTextField="name" DataValueField="name"
                                        visible="true" >
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td><asp:Label Visible="false" ID="lblMake" runat="server" text="Make:" /></td>
                <td>
                    <asp:DropDownList ID="ddMake" runat="server" AutoPostBack="True" 
                                      DataSourceID="MakeDS" DataTextField="name" DataValueField="name"
                                      visible="false">
                    </asp:DropDownList>
                </td>
                
            </tr>
            <tr>
                <td><asp:Label Visible="false" ID="lblModel" runat="server" text="Model:" /></td>
                <td>
                    <asp:DropDownList  ID="ddModel" runat="server" AutoPostBack="True" 
                                       DataSourceID="ModelDS" DataTextField="name" DataValueField="name"
                                       visible="false">
                </asp:DropDownList></td>
            </tr>
            
            <tr>
            
                <td><asp:Label Visible="false" ID="lblEngine" runat="server" text="Engine:" /></td>
                <td>
                    <asp:DropDownList   ID="ddEngine" runat="server" AutoPostBack="True" 
                                        DataSourceID="EngineDS" DataTextField="name" DataValueField="name"
                                        visible="false">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        
        </div>

        <asp:Repeater ID="rptContent" runat="server" DataMember="DefaultView" 
            DataSourceID="ContentDS">
                <HeaderTemplate> 
					<div class="tablediv">
						<div class="rowdiv">
							<div class="headerdiv">
								 <span class="appGuideTitle" >Single</span>
							</div>
							<div class="headerdiv">
								 <span class="appGuideTitle" >4 Pack</span>
							</div>
							<div class="headerdiv">
								 <span class="appGuideTitle" >Plug Type</span>
							</div>
							<div class="headerdiv">
								<span class="appGuideTitle" >Pre-Gap Setting</span>
							</div>
							<div class="headerdiv">
								<span class="appGuideTitle" >Notes</span>
							</div>
                            <div class="headerdiv">
								<span class="appGuideTitle" ></span>
							</div>
						</div>
					</div>
                </HeaderTemplate> 
                <ItemTemplate>
                    <div class="tablediv">
                        
                        <div class="rowdiv">
                            <div class="celldiv">
                                <asp:Literal ID="lit1" Text='<%# DataBinder.Eval(Container.DataItem, "MSDPNSgl") %>' runat="server" />
                            </div>
                            <div class="celldiv">
                                <asp:Literal ID ="lit2" Text='<%# DataBinder.Eval(Container.DataItem, "MSDPN4Pack") %>' runat="server" />
                            </div>
                            <div class="celldiv">
                                <asp:Literal ID="lit3" Text='<%# DataBinder.Eval(Container.DataItem, "PlugType")%>' runat="server" />
                            </div>
                            <div class="celldiv">
                                <asp:Literal ID="lit4" Text='<%# DataBinder.Eval(Container.DataItem, "PlugGap") %>' runat="server" />
                            </div>                       
							<div class="celldiv">
                                <asp:Literal ID="lit5" Text='<%# DataBinder.Eval(Container.DataItem, "Notes") %>' runat="server" />
                            </div> 
                            <div class="celldiv">
                                <asp:Literal ID="lit6" Text='<%# getSparkImage(DataBinder.Eval(Container.DataItem, "PlugType"))%>' runat="server" />
                                <div>Click to enlarge</div>
                            </div> 
                        </div>
                        
                    </div>
                   
                    <br />
                    <br />
                                       
                    

                </ItemTemplate> 
                <FooterTemplate> 
                   
                </FooterTemplate> 
        </asp:Repeater>
        


    <asp:sqldatasource runat="server" ID="ContentDS" 
            ConnectionString="<%$ ConnectionStrings:ACC_ApplicationGuideConnectionString %>" 
            SelectCommand="SELECT [id], [Make], [year], [model], [engsize], [notes], [plugtype], [msdpnsgl], [msdpn4pack], [pluggap] FROM [sparkplugdata] WHERE (([Make] = @Make) AND ([model] = @Model) AND ([year] = @Year) AND ([engsize] = @Engsize)) "
            
        ProviderName="<%$ ConnectionStrings:ACC_ApplicationGuideConnectionString.ProviderName %>" >
            <SelectParameters>
                <asp:ControlParameter ControlID="ddMake" Name="Make" 
                    PropertyName="SelectedValue" Type="String" />
                <asp:ControlParameter ControlID="ddModel" Name="model" 
                    PropertyName="SelectedValue" Type="String" />
                <asp:ControlParameter ControlID="ddYear" Name="year" 
                    PropertyName="SelectedValue" Type="String" />
                <asp:ControlParameter ControlID="ddEngine" Name="engsize" 
                    PropertyName="SelectedValue" Type="String" />
            </SelectParameters>
    </asp:sqldatasource>
        <asp:SqlDataSource ID="MakeDS" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ACC_ApplicationGuideConnectionString %>" 
            SelectCommand="SELECT [id], [name], [year] FROM [vwMakeSpark] WHERE ([year] = @year) or (id=0) ORDER BY name" 
            
        ProviderName="<%$ ConnectionStrings:ACC_ApplicationGuideConnectionString.ProviderName %>">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddYear" Name="year" 
                    PropertyName="SelectedValue" Type="String" />
            </SelectParameters>
    </asp:SqlDataSource>
        <asp:SqlDataSource ID="ModelDS" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ACC_ApplicationGuideConnectionString %>" 
            
        SelectCommand="SELECT [name], [Make], [year] FROM [vwModelSpark] WHERE (([Make] = @Make) AND ([year] = @year) or (id=0)) ORDER BY name" 
        
        ProviderName="<%$ ConnectionStrings:ACC_ApplicationGuideConnectionString.ProviderName %>">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddMake" Name="Make" 
                    PropertyName="SelectedValue" Type="String" />
                <asp:ControlParameter ControlID="ddYear" Name="year" 
                    PropertyName="SelectedValue" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="YearDS" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ACC_ApplicationGuideConnectionString %>" 
            
            
        SelectCommand="SELECT [id], [name] FROM [yearspark] order by id" 
        
        ProviderName="<%$ ConnectionStrings:ACC_ApplicationGuideConnectionString.ProviderName %>">
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="EngineDS" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ACC_ApplicationGuideConnectionString %>" 
            
            
        SelectCommand="SELECT [name] FROM [vwEngineSpark] WHERE (([year] = @Year) AND ([model] = @Model) AND([make] = @Make)) OR (year='0')" 
        ProviderName="<%$ ConnectionStrings:ACC_ApplicationGuideConnectionString.ProviderName %>">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddYear" Name="year" 
                    PropertyName="SelectedValue" Type="String" />
                <asp:ControlParameter ControlID="ddModel" Name="model" 
                    PropertyName="SelectedValue" Type="String" />
				<asp:ControlParameter ControlID="ddMake" Name="make" 
                    PropertyName="SelectedValue" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="ContentLookupDS" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ACC_ApplicationGuideConnectionString %>" 
            SelectCommand="SELECT [contentid], [partname] FROM [content_lookup]" 
        ProviderName="<%$ ConnectionStrings:ACC_ApplicationGuideConnectionString.ProviderName %>">
        </asp:SqlDataSource>
    <asp:SqlDataSource ID="BuyNowDS" runat="server" 
        ConnectionString="<%$ ConnectionStrings:Ektron.DbConnection %>" 
        SelectCommand="SELECT [meta_type_id], [content_id], [meta_value] FROM [content_meta_tbl] WHERE (([meta_type_id] = @meta_type_id) AND ([content_id] = @content_id))">
        <SelectParameters>
            <asp:Parameter DefaultValue="130" Name="meta_type_id" Type="Int64" />
            <asp:ControlParameter ControlID="lblYear" Name="content_id" PropertyName="Text" 
                Type="Int64" />
        </SelectParameters>
    </asp:SqlDataSource>
    </form>
</asp:Content>

