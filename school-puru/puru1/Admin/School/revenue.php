<?php
	include('../attach/header_sch.php');
	$rev=0;
	$income=0;
	$late=0;
	$pending=0;
	$query=mysqli_query($con,"select fee,one_time,medium,std from sch_cls_fee");
	$i=0;
	/*while($id=mysqli_fetch_row($query)){
		$count=mysqli_num_rows(mysqli_query($con,"select * from user_sch where medium='".$id[2]."' AND std='".$id[3]."'"));
	//	echo $count;
		if($id[1]=='Per Year'){
				$rev=($id[0]*$count+$rev);
			}
			else{
				$rev=(($id[0]*12)*$count)+$rev;
			}
	}*/
	$query=mysqli_query($con,"select amount,late_fee,lflag from sch_tran");
	$i=0;
	while($jd=mysqli_fetch_row($query)){
			$income=$income+$jd[0];
		if($jd[2]=='no'){
			$late=$late+$jd[1];
		}	
	}


?>
<div class="span-right">
	<div class="main-container">
		<div class="post-header">
			<span> Revenue</span>
		</div>
	    <div class="post-content">
			<div class="post-text">
				<div class="rev-div rev-recieve">
					<div class="rev-div-header">
						Total Recieved Revenue	
					</div>
					<div class="rev-content">
						<div class="content-block">
							<div class="block-top">
								From Direct Fee
							</div>
							<div class="block-bottom">
								<?php echo $income ?>
							</div>
						</div>
						<div class="content-block">
							<div class="block-top">
								From late Fee
							</div>
							<div class="block-bottom">
								<?php echo $late ?>
							</div>
						</div>
					</div>
				</div>
				<div class="rev-div rev-pend" style="cursor:pointer">
					<div class="rev-div-header">
						Total Pending
					</div>
					<div class="rev-content">

					</div>
				</div>
			</div>
		</div>
	</div>
	<!-- Added By Ranjeet ||	filter -->
	<div id="revenue_filter" class='hide'>
	
  <div class="filter-div"><span>Sec :</span>
			<select id="div-update-show" ></select> 
   </div>
  <div class="filter-div"><span>Std :</span>
  	<select id="drpStand" name="sch_ads_std" onchange="getdiv_count()" disabled>
							<option value="default">Select One</option>
							 <option value="Mr.dextro">Mr.dextro</option>
							 <option value="nursery">Nursery</option>
                             <option value="junior.kg">jr.kg</option>
                             <option value="senior.kg">sr.kg</option>
							 <option value="first">First</option>
							 <option value="second">Second</option>
							 <option value="third">Third</option>
							 <option value="fourth">Fourth</option>
							 <option value="fifth">Fifth</option>
							 <option value="sixth">Sixth</option>
							 <option value="seventh">Seventh</option>
							 <option value="eighth">Eighth</option>
							 <option value="ninth">Ninth</option>
							 <option value="tenth">Tenth</option>
    </select>
</div>

<div class="filter-div"><span>Med :</span>
		<select id="drpMedium" name="sch_ads_mdm" onchange="enable_std()">
							 <option value="default">Select One</option>
							 <option value="English">English</option>
							 <option value="Hindi">Hindi</option>
							 <option value="Marathi">Marathi</option>
 		 </select>
</div>
<div  style="width:185px;float:right;"><span>To Month :</span>
		<select id="drpTMonth" name="sch_ads_tMonth" >
							 <option value="0">Select One</option>
							 <option value="1">Jul</option>
							 <option value="2">Aug</option>
							 <option value="3">Sep</option>
							 <option value="4">Oct</option>
							 <option value="5">Nov</option>
							 <option value="6">Dec</option>
							 <option value="7">Jan</option>
							 <option value="8">Feb</option>
							 <option value="9">Mar</option>
							 <option value="10">Apr</option>
							 <option value="11">May</option>
							 <option value="12">Jun</option>
 		 </select>
</div>
<div style="width:185px;float:right;"><span>From Month :</span>
		<select id="drpFMonth" name="sch_ads_fMonth" >
							 <option value="0">Select One</option>
							 <option value="1">Jul</option>
							 <option value="2">Aug</option>
							 <option value="3">Sep</option>
							 <option value="4">Oct</option>
							 <option value="5">Nov</option>
							 <option value="6">Dec</option>
							 <option value="7">Jan</option>
							 <option value="8">Feb</option>
							 <option value="9">Mar</option>
							 <option value="10">Apr</option>
							 <option value="11">May</option>
							 <option value="12">Jun</option>
 		 </select>
</div>

<div><!--	end of filter -->

	<div class="rev-div-main">
	</div>
</div>
	<?php
	include('../attach/footer_sch.php');
?>
<script>

//on change enable std dropdown
function enable_std(){
	var drpMedium=$('#drpMedium').val();
	$('#div-update-show').html("");
	//alert(drpMedium)
	if(drpMedium !="default" && drpMedium != "" && drpMedium!= undefined)
	{
		//enable std dropdown
		 document.getElementById("drpStand").disabled=false;
	}
}

/* division dynamic filter||Added By Ranjeet */
function getdiv_count(){
	var mdm=$("select[name='sch_ads_mdm']").val();
	var std=$("select[name='sch_ads_std']").val();
	//alert(mdm);
//	console.log(mdm,std)
if(mdm != "" && mdm != undefined && std != "" && std != undefined)
{
	$.ajax({
		url:'db/filter.php',
		method:'POST',
		datatype:'json',
		data:{
			mdm:mdm,
			std:std
		},
		success:function(e){
			//alert(e);
			try{
			var div=JSON.parse(e);
			var block='';
			block +='<option value="-1">-select-</option>'
			$.each(div['data'],function(index,value){
				block +='<option value='+value+'>'+value+'</option>'
			});
			block=block+'</select>';
			//console.log(block);
			$("#div-update-show").html(block);
		}
		catch(ex)
		{console.log("Error for division filter data:"+ex);}
	}});
}
else
{
	alert("Please Select Medium and Standard.")
}
}
</script>