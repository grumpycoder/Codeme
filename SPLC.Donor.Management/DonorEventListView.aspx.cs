﻿using System;
using System.Web.UI.WebControls;
using SPLC.Donor.Models;

namespace SPLC.Donor.Management
{
    public partial class DonorEventListView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                var eventList = new EventList(User.Identity.Name);

                ddlEvents.DataSource = eventList.GetEvents();
                ddlEvents.DataTextField = "EName";
                ddlEvents.DataValueField = "pk_Event";
                ddlEvents.DataBind();

                DonorEventList DEL = new DonorEventList(User.Identity.Name);

                gvDonorEvents.DataSource = DEL.GetDonorEventList_Search(ddlEvents.SelectedIndex.ToString(), "", "",500, true);
                gvDonorEvents.DataBind();

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void LoadGrid()
        {
            DonorEventList DEL = new DonorEventList(User.Identity.Name);


            bool blShow = chMailListOnly.Checked;


            gvDonorEvents.DataSource = DEL.GetDonorEventList_Search(ddlEvents.SelectedValue.ToString(), txtDonorID.Text.ToString(), txtName.Text.ToString(), 500, blShow);
            gvDonorEvents.DataBind();

            if(blShow)
            {
                gvDonorEvents.Columns[6].Visible = true;
                gvDonorEvents.Columns[7].Visible = false;

            }
            else
            {
                gvDonorEvents.Columns[6].Visible = false;
                gvDonorEvents.Columns[7].Visible = true;
            }
        }

        protected void gvDonorEvents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvDonorEvents.Rows[index];

                int intEL = int.Parse(ddlEvents.SelectedValue.ToString());
                string strDonor = row.Cells[0].Text.ToString();

                DonorEventList DEL = new DonorEventList(User.Identity.Name);
                DEL.Load(intEL, strDonor);

                DEL.TicketsMailed_Date = DateTime.Now;
                DEL.TicketsMailed_User = User.Identity.Name;
                //DEL.TicketsRequested = int.Parse(txtAttending.Text.ToString());
                DEL.MailCards();

                LoadGrid();

                //gvDonorEvents.Rows[index].Visible = false;


            
        }
    }
}